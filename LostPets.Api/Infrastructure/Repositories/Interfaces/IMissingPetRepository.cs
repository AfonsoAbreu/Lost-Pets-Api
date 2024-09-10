using Infrastructure.Data.Entities;
using Infrastructure.Repositories.Base;
using NetTopologySuite.Geometries;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IMissingPetRepository : IBaseRepository<MissingPet>
    {
        List<MissingPet> SearchBylocationAndRadius(Point location, double radius, int page = 1, int itemsPerPage = 10);
    }
}
