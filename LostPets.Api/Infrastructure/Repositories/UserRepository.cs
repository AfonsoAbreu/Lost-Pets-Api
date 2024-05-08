using Infrastructure.Data;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories.Base;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
