using Application.Exceptions;
using Application.Services.Base;
using Application.Services.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class UserService : BaseService, IUserService
    {
        protected readonly IUserRepository _userRepository;
        protected readonly UserManager<User> _userManager;

        public UserService(ApplicationDbContext applicationDbContext, IUserRepository userRepository, UserManager<User> userManager) : base(applicationDbContext)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task CreateUserAsync(User user, string password)
        {
            IdentityResult creationResult = await _userManager.CreateAsync(user, password);

            if (!creationResult.Succeeded)
            {
                UnsuccessfulOperationDomainException exception = new UnsuccessfulOperationDomainException(UnsuccessfulOperationDomainException.DefaultMessage("Register"));

                foreach (var error in creationResult.Errors)
                {
                    exception.Data.Add(error.Code, error.Description);
                }

                throw exception;
            }
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            User? user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                LoadCommonCollections(user);
            }

            return user;
        }

        public User? GetById(Guid id, bool includeDeletedMissingPets = false)
        {
            User? user = _userRepository.GetById(id);

            if (user != null)
            {
                LoadCommonCollections(user);
                
                if (!includeDeletedMissingPets && user.MissingPets != null)
                {
                    user.MissingPets = user.MissingPets
                        .Where(missingPet => missingPet.Status != MissingPetStatusEnum.DEACTIVATED)
                        .ToList();
                }
            }

            return user;
        }

        private void LoadCommonCollections(User user)
        {
            _userRepository.ExplicitLoadCollection(user, u => u.MissingPets);
            _userRepository.ExplicitLoadCollection(user, u => u.Comments);
            _userRepository.ExplicitLoadCollection(user, u => u.Sightings);
        }
    }
}
