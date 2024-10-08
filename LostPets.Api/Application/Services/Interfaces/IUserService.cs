using Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        User? GetById(Guid id, bool includeDeletedMissingPets = false);
        Task<User?> GetByEmailAsync(string email);
        Task CreateUserAsync(User user, string password);
        void RemoveImage(User user);
        Task<Image> AddImage(User user, IFormFile file);
    }
}
