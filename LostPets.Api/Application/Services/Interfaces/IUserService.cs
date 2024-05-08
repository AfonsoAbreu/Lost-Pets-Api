using Infrastructure.Data.Entities;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        User? GetById(Guid id, bool includeDeletedMissingPets = false);
    }
}
