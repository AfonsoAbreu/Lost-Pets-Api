using Infrastructure.Data.Entities;

namespace Application.Services.Interfaces
{
    public interface ISightingService
    {
        Sighting AddOrUpdate(Sighting sighting, bool withSaveChanges = true);
        IEnumerable<Sighting> AddOrUpdate(IEnumerable<Sighting> sightings, bool withSaveChanges = true);
        Sighting Add(Sighting sighting, bool withSaveChanges = true);
        IEnumerable<Sighting> Add(IEnumerable<Sighting> sightings, bool withSaveChanges = true);
        Sighting Update(Sighting sighting, bool withSaveChanges = true);
        IEnumerable<Sighting> Update(IEnumerable<Sighting> sightings, bool withSaveChanges = true);
        Sighting? GetById(Guid id);
        void Remove(Sighting sighting);
    }
}
