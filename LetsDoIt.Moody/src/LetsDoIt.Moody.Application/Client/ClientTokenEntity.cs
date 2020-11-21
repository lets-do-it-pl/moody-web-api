using System;

namespace LetsDoIt.Moody.Application.Client
{
    public class ClientTokenEntity
    {
        public ClientTokenEntity(
            string username, 
            string token, 
            DateTime expirationDate)
        {
            Username = username;
            Token = token;
            ExpirationDate = expirationDate;
        }

        public string Username { get; }

        public string Token { get; }

        public DateTime ExpirationDate { get; }
    }
}
