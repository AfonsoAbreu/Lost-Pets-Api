using Infrastructure.Data.Entities;

namespace Application.Services.Interfaces
{
    public interface ICommentService
    {
        Comment AddOrUpdate(Comment comment, bool withSaveChanges = true);
        IEnumerable<Comment> AddOrUpdate(IEnumerable<Comment> comments, bool withSaveChanges = true);
        Comment Add(Comment comment, bool withSaveChanges = true);
        IEnumerable<Comment> Add(IEnumerable<Comment> comments, bool withSaveChanges = true);
        Comment Update(Comment comment, bool withSaveChanges = true);
        IEnumerable<Comment> Update(IEnumerable<Comment> comments, bool withSaveChanges = true);
    }
}
