using Infrastructure.Data.Entities;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        User? GetById(Guid id, bool includeDeletedMissingPets = false);
        Task<User?> GetByEmailAsync(string email);
        Task CreateUserAsync(User user, string password);
    }
}
