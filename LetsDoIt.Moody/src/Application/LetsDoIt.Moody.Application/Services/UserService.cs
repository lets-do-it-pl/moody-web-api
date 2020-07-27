using System;
using System.Text;
using System.Security.Claims;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using LetsDoIt.Moody.Domain;
using Microsoft.Extensions.Configuration;

namespace LetsDoIt.Moody.Application.Services
{ 

    public class UserService : IUserService
    {
        private readonly string key;
        private readonly int tokenExpirationMin;

        public UserService(string key, int tokenExpirationMin)
        {
            this.key = key;
            this.tokenExpirationMin = tokenExpirationMin;
        }

        //For testing : unnecessary
        private readonly List<string> users = new List<string>
        {
            {"4w3A6H263XZQGo1hFaAciFdiQg/nTxSeWhANED2PA5Q="},
            {"jsadgsajhfggfsakgsakk" }
        };

        public string EncryptUserNameAndPassword(string username, string password)
        {
            string user = username + password;
            byte[] salt = new byte[128/8];
      
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: user,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hashed;
        }

        public string Authenticate(string login)
        {
            var context = new DataService();

            if (!users.Any(u => u.Equals(login)))
            {
                throw new Exception("User does not exists");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,login)
                }),
                Expires = DateTime.UtcNow.AddMinutes(tokenExpirationMin),
                SigningCredentials =
                    new SigningCredentials(
                        new SymmetricSecurityKey(tokenKey),
                        SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }


    }
}
