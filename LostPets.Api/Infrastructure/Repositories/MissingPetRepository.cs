using Infrastructure.Data;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories.Base;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Infrastructure.Repositories
{
    public class MissingPetRepository : BaseRepository<MissingPet>, IMissingPetRepository
    {
        public MissingPetRepository(ApplicationDbContext context) : base(context)
        {

        }

        public List<MissingPet> SearchBylocationAndRadius(Point location, double radius, int page = 1, int itemsPerPage = 10)
        {
            return GetSet()
                .Where(missingPet =>
                    missingPet.Sightings.Any(sighting => sighting.Location.IsWithinDistance(location, radius))
                    && missingPet.Status == MissingPetStatusEnum.LOST
                )
                .OrderBy(missingPet => missingPet.Id)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .Include(missingPet => missingPet.Sightings)
                .Include(missingPet => missingPet.Comments)
                .ToList();
        }
    }
}
