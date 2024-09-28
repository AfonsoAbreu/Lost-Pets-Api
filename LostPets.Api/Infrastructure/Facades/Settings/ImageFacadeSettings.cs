using Infrastructure.Facades.Settings.Interfaces;

namespace Infrastructure.Facades.Settings
{
    public class ImageFacadeSettings : IBaseFacadeSettings
    {
        public string SectionName => "ImageSettings";

        public string UploadsPath { get; init; }
        public string? UploadsSubFolder { get; init; }
        public int MaxImagesPerMissingPet { get; init; }
        public IEnumerable<string>? AllowedImageTypes { get; init; }
    }
}