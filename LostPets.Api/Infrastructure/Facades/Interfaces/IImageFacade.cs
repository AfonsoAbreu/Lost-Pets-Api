using Microsoft.AspNetCore.Http;

namespace Infrastructure.Facades.Interfaces
{
    public interface IImageFacade
    {
        Task<string> SaveImage(IFormFile formFile, string fileName);
        void DeleteImage(string filePath);
    }
}
