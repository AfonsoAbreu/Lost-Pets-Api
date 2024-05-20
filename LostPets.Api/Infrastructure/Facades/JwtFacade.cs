using Infrastructure.Data.Entities;
using Infrastructure.Facades.Base;
using Infrastructure.Facades.Interfaces;
using Infrastructure.Facades.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Facades
{
    public class JwtFacade : BaseFacadeWithSettings<JwtFacadeSettings>, IJwtFacade
    {
        public JwtFacade(IOptions<JwtFacadeSettings> options) : base(options)
        {
        }

        public string GenerateJwt(User user)
        {
            SigningCredentials signingCredentials = new(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret)),
                SecurityAlgorithms.HmacSha256
            );

            IEnumerable<Claim> claims =
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
            ];

            JwtSecurityToken securityToken = new(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes),
                claims: claims,
                signingCredentials: signingCredentials
            );

            JwtSecurityTokenHandler handler = new();
            string token = handler.WriteToken(securityToken);

            return token;
        }
    }
}
