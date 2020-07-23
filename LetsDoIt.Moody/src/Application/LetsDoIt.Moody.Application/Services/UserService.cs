using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace LetsDoIt.Moody.Application.Services
{

    public class UserService : IUserService
    {
        private readonly IDictionary<string, string> users = new Dictionary<string, string>
        {
            {
                "test1", "password1"
            },
            {
                "test2", "password2"
            }
        };

        private readonly string key;
        private string username;
        private string password;
        private readonly IConfiguration _config;

        public UserService(IConfiguration config)
        {
            _config = config;
        }

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

        public void CheckUserExists()
        {
            if (!users.Any(u => u.Key == username && u.Value == password))
            {
                throw new Exception("User does not exists");
            }

            Authenticate("", "");
        }

        public string Authenticate(string username, string password)
        {
            /*var context = new DataService();
            var User = new UserService(username, password);

            if (!context.Users.Any(user => user.UsernameAndPassword == User.EncryptUserNameAndPassword()))
             {
                return null;
             }*/

            if (!users.Any(u => u.Key == username && u.Value == password))
            {
                throw new Exception("User does not exists");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,username)
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                //(_config.GetValue<int>(
                //"ExprationDateOfJWT_InMinutes")),
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
