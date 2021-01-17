using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LetsDoIt.Moody.Application.Security
{
    using Options;

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
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Name, fullName),
                new Claim(ClaimTypes.Role,role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtOptions.TokenExpirationMinutes),
                signingCredentials: credentials
            );

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenInfo(tokenValue, token.ValidTo);
        }
    }
}
