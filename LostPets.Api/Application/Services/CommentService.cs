using Application.Exceptions;
using Application.Services.Base;
using Application.Services.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories.Interfaces;

namespace Application.Services
{
    public class CommentService : BaseService, ICommentService
    {
        protected readonly ICommentRepository _commentRepository;

        public CommentService(ApplicationDbContext applicationDbContext, ICommentRepository commentRepository) : base(applicationDbContext)
        {
            _commentRepository = commentRepository;
        }

        public Comment AddOrUpdate(Comment comment, bool withSaveChanges = true)
        {
            try
            {
                return Update(comment, withSaveChanges);
            }
            catch (ResourceNotFoundDomainException)
            {
                return Add(comment, withSaveChanges);
            }
        }

        public Comment Add(Comment comment, bool withSaveChanges = true)
        {
            _commentRepository.Add(comment);

            if (withSaveChanges)
            {
                SaveChanges();
            }

            return comment;
        }

        public Comment Update(Comment comment, bool withSaveChanges = true)
        {
            Comment? existingComment = _commentRepository.GetById(comment.Id);

            if (existingComment == null)
            {
                throw new ResourceNotFoundDomainException(ResourceNotFoundDomainException.DefaultMessage("Comment"));
            }
            else if (existingComment.UserId != comment.UserId)
            {
                throw new MismatchedUserDomainException(MismatchedUserDomainException.DefaultMessage("Comment"));
            }
            else if (existingComment.MissingPetId != comment.MissingPetId)
            {
                throw new MismatchedRelationDomainException(MismatchedRelationDomainException.DefaultMessage("Comment", "missingPetId"));
            }
            else if (existingComment.AwnsersTo != comment.AwnsersTo)
            {
                throw new MismatchedRelationDomainException(MismatchedRelationDomainException.DefaultMessage("Comment", "awnsersTo"));
            }

            existingComment.Content = comment.Content;

            if (withSaveChanges)
            {
                SaveChanges();
            }

            return existingComment;
        }

        public IEnumerable<Comment> AddOrUpdate(IEnumerable<Comment> comments, bool withSaveChanges = true)
        {
            return comments.Select(comment => AddOrUpdate(comment, withSaveChanges)).ToList();
        }

        public IEnumerable<Comment> Add(IEnumerable<Comment> comments, bool withSaveChanges = true)
        {
            return comments.Select(comment => Add(comment, withSaveChanges)).ToList();
        }

        public IEnumerable<Comment> Update(IEnumerable<Comment> comments, bool withSaveChanges = true)
        {
            return comments.Select(comment => Update(comment, withSaveChanges)).ToList();
        }
    }
}
