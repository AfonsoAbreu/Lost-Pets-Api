using Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Http;
using NetTopologySuite.Geometries;

namespace Application.Services.Interfaces
{
    public interface IMissingPetService
    {
        void Add(MissingPet missingPet);
        IEnumerable<MissingPet> SearchBylocationAndRadius(Point location, double radius, int page = 1, int itemsPerPage = 10);
        MissingPet? GetById(Guid id);
        MissingPet Update(MissingPet missingPet);
        void Remove(MissingPet missingPet);
        void Deactivate(MissingPet missingPet);
        IAsyncEnumerable<Image> AddImage(MissingPet missingPet, IEnumerable<IFormFile> formFile);
        void RemoveImage(MissingPet missingPet, Image image);
    }
}
