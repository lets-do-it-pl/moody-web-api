using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace LetsDoIt.Moody.Application.Services
{

    public class UserService : IUserService
    {
        private readonly string key;
        private string username;
        private string password;

        public UserService(string key)
        {
            this.key = key;
        }

        public UserService(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public string EncryptUserNameAndPassword()
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

        public string Authenticate(string username, string password)
        {
           /* if (!users.Any(user => user.name == Encrypt(username, password)))
            {
                return null;
            }*/

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,username)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
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
