using Infrastructure.Data.Entities;

namespace Application.Services.Interfaces
{
    public interface IPetService
    {
        Pet Update(Pet pet, bool withSaveChanges = true);
    }
}
