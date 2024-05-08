using Application.Exceptions;
using Application.Services.Base;
using Application.Services.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories.Interfaces;

namespace Application.Services
{
    public class UserService : BaseService, IUserService
    {
        protected readonly IUserRepository _userRepository;

        public UserService(ApplicationDbContext applicationDbContext, IUserRepository userRepository) : base(applicationDbContext)
        {
            _userRepository = userRepository;
        }

        public User? GetById(Guid id, bool includeDeletedMissingPets = false)
        {
            User? user = _userRepository.GetById(id);

            if (user != null)
            {
                _userRepository.ExplicitLoadCollection(user, u => u.MissingPets);
                _userRepository.ExplicitLoadCollection(user, u => u.Comments);
                _userRepository.ExplicitLoadCollection(user, u => u.Sightings);
                
                if (includeDeletedMissingPets && user.MissingPets != null)
                {
                    user.MissingPets = user.MissingPets
                        .Where(missingPet => missingPet.Status != MissingPetStatusEnum.DEACTIVATED)
                        .ToList();
                }
            }

            return user;
        }
    }
}
