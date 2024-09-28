using Infrastructure.Data.Entities;
using Infrastructure.Repositories.Base;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IImageRepository : IBaseRepository<Image>
    {
        public Task<Image> SaveImage(IFormFile file);
    }
}
