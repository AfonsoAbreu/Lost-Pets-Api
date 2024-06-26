﻿using Application.Exceptions;
using Application.Services.Base;
using Application.Services.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories.Interfaces;
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

        public MissingPetService(ApplicationDbContext applicationDbContext, IMissingPetRepository missingPetRepository, IPetService petService, ISightingService sightingService, ICommentService commentService, ICommentRepository commentRepository, IPetRepository petRepository, ISightingRepository sightingRepository) : base(applicationDbContext)
        {
            _missingPetRepository = missingPetRepository;
            _sightingRepository = sightingRepository;
            _commentRepository = commentRepository;
            _petRepository = petRepository;
            _petService = petService;
            _sightingService = sightingService;
            _commentService = commentService;
        }

        public void Add(MissingPet missingPet)
        {
            if (missingPet.Sightings.Count == 0)
            {
                throw new ValidationDomainException("A MissingPet must have at least one Sighting");
            }
            else if (missingPet.Comments == null || missingPet.Comments.Count != 0)
            {
                throw new ValidationDomainException("A MissinPet must not be created with Comments.");
            }

            _missingPetRepository.Add(missingPet);

            SaveChanges();
        }

        public MissingPet? GetById(Guid id)
        {
            MissingPet? missingPet = _missingPetRepository.GetById(id);

            if (missingPet != null)
            {
                _missingPetRepository.ExplicitLoadCollection(missingPet, entity => entity.Sightings);
                _missingPetRepository.ExplicitLoadCollection(missingPet, entity => entity.Comments);
            }

            return missingPet;
        }

        public IEnumerable<MissingPet> SearchBylocationAndRadius(Point location, double radius, int page = 1, int itemsPerPage = 10)
        {
            IEnumerable<MissingPet> missingPets = _missingPetRepository.SearchBylocationAndRadius(location, radius, page, itemsPerPage);
            _missingPetRepository.Detach(missingPets);

            foreach (MissingPet missingPet in missingPets)
            {
                if (missingPet.Comments != null)
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

            if (missingPet.Comments != null && missingPet.Comments.Count != 0)
            {
                existingMissingPet.Comments = existingMissingPet.Comments?.Union(_commentService.AddOrUpdate(missingPet.Comments, false))?.ToList();
            }
            
            SaveChanges();

            if (existingMissingPet.Comments != null)
            {
                existingMissingPet.Comments = _commentService.FilterByRootLevelComments(existingMissingPet.Comments).ToList();
            }

            return existingMissingPet;
        }

        public void Remove(MissingPet missingPet)
        {
            _commentRepository.RemoveWhere(comment => comment.MissingPetId == missingPet.Id);
            _sightingRepository.RemoveWhere(sighting => sighting.MissingPetId == missingPet.Id);

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
    }
}
