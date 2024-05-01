using Infrastructure.Data;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories.Base;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories
{
    public class SightingRepository : BaseRepository<Sighting>, ISightingRepository
    {
        public SightingRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
