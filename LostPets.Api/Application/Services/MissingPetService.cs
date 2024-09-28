using Application.Exceptions;
using Application.Services.Base;
using Application.Services.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Entities;
using Infrastructure.Facades.Settings;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetTopologySuite.Geometries;

namespace Application.Services
{
    public class MissingPetService : BaseService, IMissingPetService
    {
        protected readonly IMissingPetRepository _missingPetRepository;
        protected readonly ICommentRepository _commentRepository;
        protected readonly ISightingRepository _sightingRepository;
        protected readonly IPetRepository _petRepository;
        protected readonly IPetService _petService;
        protected readonly ISightingService _sightingService;
        protected readonly ICommentService _commentService;
        protected readonly IImageRepository _imageRepository;
        protected readonly IMissingPetImageRepository _missingPetImageRepository;

        protected readonly ImageFacadeSettings _imageFacadeSettings;

        public MissingPetService(
            ApplicationDbContext applicationDbContext, 
            IMissingPetRepository missingPetRepository, 
            IPetService petService, 
            ISightingService sightingService, 
            ICommentService commentService,
            ICommentRepository commentRepository,
            IPetRepository petRepository, 
            ISightingRepository sightingRepository,
            IImageRepository imageRepository,
            IOptions<ImageFacadeSettings> imageOptions,
            IMissingPetImageRepository missingPetImageRepository
            ) : base(applicationDbContext)
        {
            _missingPetRepository = missingPetRepository;
            _sightingRepository = sightingRepository;
            _commentRepository = commentRepository;
            _petRepository = petRepository;
            _petService = petService;
            _sightingService = sightingService;
            _commentService = commentService;
            _imageRepository = imageRepository;
            _imageFacadeSettings = imageOptions.Value;
            _missingPetImageRepository = missingPetImageRepository;
        }

        public void Add(MissingPet missingPet)
        {
            if (missingPet.Sightings.Count == 0)
            {
                throw new ValidationDomainException("A MissingPet must have at least one Sighting.");
            }
            else if (missingPet.Comments == null || missingPet.Comments.Count != 0)
            {
                throw new ValidationDomainException("A MissingPet must not be created with Comments.");
            }

            _missingPetRepository.Add(missingPet);

            SaveChanges();
        }

        public MissingPet? GetById(Guid id)
        {
            MissingPet? missingPet = _missingPetRepository.GetById(id);

            if (missingPet != null)
            {
                _missingPetRepository.ExplicitLoadCollection(missingPet, entity => entity.Sightings, query => query.Include(sighting => sighting.User));
                _missingPetRepository.ExplicitLoadCollection(missingPet, entity => entity.Comments, query => query.Include(comment => comment.User));
            }

            return missingPet;
        }

        public IEnumerable<MissingPet> SearchBylocationAndRadius(Point location, double radius, int page = 1, int itemsPerPage = 10)
        {
            List<MissingPet> missingPets = _missingPetRepository.SearchBylocationAndRadius(location, radius, page, itemsPerPage);
            _missingPetRepository.Detach(missingPets);

            foreach (MissingPet missingPet in missingPets)
            {
                if (!missingPet.Comments.IsNullOrEmpty())
                {
                    missingPet.Comments = _commentService.FilterByRootLevelComments(missingPet.Comments).ToList();
                }

                yield return missingPet;
            }
        }

        public MissingPet Update(MissingPet missingPet)
        {
            MissingPet? existingMissingPet = GetById(missingPet.Id);

            if (existingMissingPet == null)
            {
                throw new ResourceNotFoundDomainException(ResourceNotFoundDomainException.DefaultMessage("Missing Pet"));
            } 
            else if (existingMissingPet.UserId != missingPet.UserId)
            {
                throw new MismatchedUserDomainException(MismatchedUserDomainException.DefaultMessage("Missing Pet"));
            }

            existingMissingPet.Status = missingPet.Status;

            missingPet.Pet.Id = existingMissingPet.PetId;

            existingMissingPet.Pet = _petService.Update(missingPet.Pet, false);

            if (missingPet.Sightings.Count != 0)
            {
                existingMissingPet.Sightings = existingMissingPet.Sightings.Union(_sightingService.AddOrUpdate(missingPet.Sightings, false)).ToList();
            }

            if (!missingPet.Comments.IsNullOrEmpty())
            {
                existingMissingPet.Comments = existingMissingPet.Comments?.Union(_commentService.AddOrUpdate(missingPet.Comments, false))?.ToList();
            }
            
            SaveChanges();

            if (!existingMissingPet.Comments.IsNullOrEmpty())
            {
                existingMissingPet.Comments = _commentService.FilterByRootLevelComments(existingMissingPet.Comments).ToList();
            }

            return existingMissingPet;
        }

        public void Remove(MissingPet missingPet)
        {
            //TODO: Fiz all of the RemoveWhere calls
            _commentRepository.RemoveWhere(comment => comment.MissingPetId == missingPet.Id);
            _sightingRepository.RemoveWhere(sighting => sighting.MissingPetId == missingPet.Id);

            if (missingPet.Images != null && missingPet.MissingPetImages != null && missingPet.Images.Count > 0)
            {
                _missingPetImageRepository.RemoveWhere(image => missingPet.MissingPetImages.Contains(image));
                _imageRepository.RemoveWhere(image => missingPet.Images.Contains(image));
            }

            _petRepository.Remove(missingPet.Pet);
            _missingPetRepository.Remove(missingPet);

            SaveChanges();
        }

        public void Deactivate(MissingPet missingPet)
        {
            _missingPetRepository.Attach(missingPet);
            missingPet.Status = MissingPetStatusEnum.DEACTIVATED;
            SaveChanges();
        }

        public async IAsyncEnumerable<Image> AddImage(MissingPet missingPet, IEnumerable<IFormFile> formFile)
        {
            int remainingSlots = _imageFacadeSettings.MaxImagesPerMissingPet;

            if (missingPet.Images != null)
            {
                remainingSlots -= missingPet.Images.Count;
            }

            if (remainingSlots < formFile.Count())
            {
                throw new ValidationDomainException($"A MissingPet cannot have more than {_imageFacadeSettings.MaxImagesPerMissingPet} images.");
            }

            foreach (var file in formFile)
            {
                Image image = await _imageRepository.SaveImage(file);

                if (missingPet.Images == null)
                {
                    missingPet.Images = new List<Image>();
                }

                missingPet.Images.Add(image);

                yield return image;
            }

            SaveChanges();
        }

        public void RemoveImage(MissingPet missingPet, Image image)
        {
            MissingPetImage? missingPetImage = missingPet.MissingPetImages
                ?.Where(mpi => mpi.ImageId == image.Id)
                .FirstOrDefault();

            if (missingPetImage == null)
            {
                throw new ResourceNotFoundDomainException(ResourceNotFoundDomainException.DefaultMessage("Image"));
            }

            _missingPetImageRepository.Remove(missingPetImage);
            _imageRepository.Remove(image);

            SaveChanges();
        }
    }
}
