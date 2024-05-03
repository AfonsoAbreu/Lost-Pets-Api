using Application.Exceptions;
using Application.Services.Base;
using Application.Services.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories.Interfaces;

namespace Application.Services
{
    public class SightingService : BaseService, ISightingService
    {
        protected readonly ISightingRepository _sightingRepository;

        public SightingService(ApplicationDbContext applicationDbContext, ISightingRepository sightingRepository) : base(applicationDbContext)
        {
            _sightingRepository = sightingRepository;
        }

        public Sighting AddOrUpdate(Sighting sighting, bool withSaveChanges = true)
        {
            try
            {
                return Update(sighting, withSaveChanges);
            }
            catch (ResourceNotFoundDomainException)
            {
                return Add(sighting, withSaveChanges);
            }
        }

        public Sighting Add(Sighting sighting, bool withSaveChanges = true)
        {
            _sightingRepository.Add(sighting);

            if (withSaveChanges)
            {
                SaveChanges();
            }

            return sighting;
        }

        public Sighting? GetById(Guid id)
        {
            Sighting? sighting = _sightingRepository.GetById(id);

            return sighting;
        }

        public Sighting Update(Sighting sighting, bool withSaveChanges = true)
        {
            Sighting? existingSighting = _sightingRepository.GetById(sighting.Id);

            if (existingSighting == null)
            {
                throw new ResourceNotFoundDomainException(ResourceNotFoundDomainException.DefaultMessage("Sighting"));
            }
            else if (existingSighting.UserId != sighting.UserId)
            {
                throw new MismatchedUserDomainException(MismatchedUserDomainException.DefaultMessage("Sighting"));
            }
            else if (existingSighting.MissingPetId != sighting.MissingPetId)
            {
                throw new MismatchedRelationDomainException(MismatchedRelationDomainException.DefaultMessage("Sighting", "missingPetId"));
            }

            existingSighting.Description = sighting.Description;
            existingSighting.Location = sighting.Location;
            existingSighting.SightingDate = sighting.SightingDate;

            if (withSaveChanges)
            {
                SaveChanges();
            }

            return existingSighting;
        }

        public IEnumerable<Sighting> AddOrUpdate(IEnumerable<Sighting> sightings, bool withSaveChanges = true)
        {
            return sightings.Select(sighting => AddOrUpdate(sighting, withSaveChanges)).ToList();
        }

        public IEnumerable<Sighting> Add(IEnumerable<Sighting> sightings, bool withSaveChanges = true)
        {
            return sightings.Select(sighting => Add(sighting, withSaveChanges)).ToList();
        }

        public IEnumerable<Sighting> Update(IEnumerable<Sighting> sightings, bool withSaveChanges = true)
        {
            return sightings.Select(sighting => Update(sighting, withSaveChanges)).ToList();
        }

        public void Remove(Sighting sighting)
        {
            _sightingRepository.Remove(sighting);
            SaveChanges();
        }
    }
}
