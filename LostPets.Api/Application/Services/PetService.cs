using Application.Exceptions;
using Application.Services.Base;
using Application.Services.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories.Interfaces;

namespace Application.Services
{
    public class PetService : BaseService, IPetService
    {
        protected readonly IPetRepository _petRepository;

        public PetService(ApplicationDbContext applicationDbContext, IPetRepository petRepository) : base(applicationDbContext)
        {
            _petRepository = petRepository;
        }

        public Pet Update(Pet pet, bool withSaveChanges = true)
        {
            Pet? existingPet = _petRepository.GetById(pet.Id);

            if (existingPet == null)
            {
                throw new ResourceNotFoundDomainException(ResourceNotFoundDomainException.DefaultMessage("Pet"));
            }

            existingPet.Description = pet.Description;
            existingPet.Name = pet.Name;
            existingPet.Age = pet.Age;
            existingPet.Species = pet.Species;

            if (withSaveChanges)
            {
                SaveChanges();
            }

            return existingPet;
        }
    }
}
