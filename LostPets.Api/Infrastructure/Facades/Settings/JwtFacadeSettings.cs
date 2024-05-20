using Infrastructure.Facades.Settings.Interfaces;

namespace Infrastructure.Facades.Settings
{
    public class JwtFacadeSettings : IBaseFacadeSettings
    {
        public string SectionName => "JwtSettings";

        public string Secret { get; init; }
        public int ExpiryMinutes { get; init; }
        public string? Issuer { get; set; } = null;
        public string? Audience { get; set; } = null;
    }
}
