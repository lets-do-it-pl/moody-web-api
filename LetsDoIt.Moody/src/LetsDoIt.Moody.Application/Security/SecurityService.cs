using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using LetsDoIt.Moody.Application.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LetsDoIt.Moody.Application.Security
{
    public class SecurityService : ISecurityService
    {
        private readonly JwtOptions _jwtOptions;

        public SecurityService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public TokenInfo GenerateJwtToken(string id, string fullName, string role)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Name, fullName),
                new Claim(ClaimTypes.Role,role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtOptions.TokenExpirationMinutes),
                signingCredentials: credentials
            );

            return new TokenInfo
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpirationDate = token.ValidTo
            };
        }
    }
}
