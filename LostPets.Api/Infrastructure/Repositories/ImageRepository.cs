using Crosscutting.ExtensionMethods;
using Infrastructure.Data;
using Infrastructure.Data.Entities;
using Infrastructure.Facades.Interfaces;
using Infrastructure.Facades.Settings;
using Infrastructure.Repositories.Base;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class ImageRepository : BaseRepository<Image>, IImageRepository
    {
        private readonly IImageFacade _imageFacade;
        private readonly ImageFacadeSettings _imageFacadeSettings;
        private readonly IServerFacade _serverFacade;

        public ImageRepository(
            ApplicationDbContext context, 
            IImageFacade imageFacade,
            IOptions<ImageFacadeSettings> options,
            IServerFacade serverFacade
        ) : base(context)
        {
            _imageFacade = imageFacade;
            _imageFacadeSettings = options.Value;
            _serverFacade = serverFacade;
        }

        public async Task<Image> SaveImage(IFormFile file)
        {
            string fileName = Guid.NewGuid().ToString();
            string fileExtension = file.GetFileExtension();
            string hostedUrl = _serverFacade.GetHostedUrl();
            string url = $"{hostedUrl}{_imageFacadeSettings.UploadsSubFolder}/{fileName}.{fileExtension}";
            string filePath = await _imageFacade.SaveImage(file, fileName);

            var image = new Image 
            { 
                Location = filePath,
                Url = url,
            };

            Add(image);

            return image;
        }

        public override void Remove(Image entity)
        {
            base.Remove(entity);

            if (entity.IsLocal)
            {
                _imageFacade.DeleteImage(entity.Location);
            }
        }

        public override void RemoveRange(IEnumerable<Image> entities)
        {
            foreach (Image entity in entities)
            {
                Remove(entity);
            }
        }

        public override int RemoveWhere(Expression<Func<Image, bool>> where)
        {
            IQueryable<Image> images = GetSet().Where(where);

            List<Image> localImages = images
                .Where(image => image.Location != null)
                .ToList();

            int result = images.ExecuteDelete();
            
            if (result == 0)
            {
                return result;
            }

            foreach (Image image in localImages)
            {
                _imageFacade.DeleteImage(image.Location);
            }

            return result;
        }
    }
}
