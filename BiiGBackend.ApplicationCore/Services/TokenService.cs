using BiiGBackend.ApplicationCore.Services.Interfaces;
using BiiGBackend.Models.Entities.Identity;
using BiiGBackend.Models.SharedModels;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BiiGBackend.ApplicationCore.Services
{
    public class TokenService : ITokenService
    {
        private JwtOptions _jwtOptions;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
            _key = new(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        }

        public string CreateToken(ApplicationUser user)
        {

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("roles",user.Role),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.NameId,user.Id.ToString())
            };

            ClaimsIdentity identity = new(claims);

            var cred = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                SigningCredentials = cred,
                Subject = identity,
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Expires = DateTime.UtcNow.AddMonths(12)
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);

        }
    }
}
