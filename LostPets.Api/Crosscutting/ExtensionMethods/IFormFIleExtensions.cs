

using Microsoft.AspNetCore.Http;

namespace Crosscutting.ExtensionMethods
{
    public static class IFormFIleExtensions
    {
        public static string GetFileExtension(this IFormFile formFile)
        {
            return formFile.ContentType.Split('/').Last();
        }
    }
}
