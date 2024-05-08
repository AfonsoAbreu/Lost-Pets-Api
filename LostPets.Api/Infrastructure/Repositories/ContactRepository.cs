using Infrastructure.Data;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories.Base;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories
{
    public class ContactRepository : BaseRepository<Contact>, IContactRepository
    {
        public ContactRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
