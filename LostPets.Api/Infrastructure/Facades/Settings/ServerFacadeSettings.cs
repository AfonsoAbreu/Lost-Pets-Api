using Infrastructure.Facades.Settings.Interfaces;

namespace Infrastructure.Facades.Settings
{
    public class ServerFacadeSettings : IBaseFacadeSettings
    {
        public string SectionName => "ServerSettings";

        public string? HostedUrl { get; init; }
    }
}
