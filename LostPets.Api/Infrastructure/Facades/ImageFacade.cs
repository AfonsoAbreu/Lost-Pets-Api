using Infrastructure.Facades.Base;
using Infrastructure.Facades.Interfaces;
using Infrastructure.Facades.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Crosscutting.ExtensionMethods;
using Infrastructure.Exceptions;

namespace Infrastructure.Facades
{
    public class ImageFacade : BaseFacadeWithSettings<ImageFacadeSettings>, IImageFacade
    {
        public ImageFacade(IOptions<ImageFacadeSettings> options) : base(options)
        {
        }

        public async Task<string> SaveImage(IFormFile formFile, string fileName)
        {
            string fileType = formFile.GetFileExtension();
            if (_settings.AllowedImageTypes != null && !_settings.AllowedImageTypes.Contains(fileType))
            {
                throw new InvalidFileTypeInfrastructureException(InvalidFileTypeInfrastructureException.DefaultMessage(fileType));
            }

            string fullFilePath = Path.Combine(_settings.UploadsPath, fileName);

            fullFilePath = Path.ChangeExtension(fullFilePath, fileType);

            using FileStream fileStream = File.Create(fullFilePath);
            await formFile.CopyToAsync(fileStream);

            return fullFilePath;
        }

        public void DeleteImage(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileDeletionInfrastructureException(FileDeletionInfrastructureException.DefaultMessage(filePath));
            }

            try
            {
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                throw new FileDeletionInfrastructureException(FileDeletionInfrastructureException.DefaultMessage(filePath), ex);
            }
        }
    }
}
