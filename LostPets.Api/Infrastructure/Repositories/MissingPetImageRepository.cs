using Infrastructure.Data;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories.Base;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories
{
    public class MissingPetImageRepository : BaseRepository<MissingPetImage>, IMissingPetImageRepository
    {
        public MissingPetImageRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
