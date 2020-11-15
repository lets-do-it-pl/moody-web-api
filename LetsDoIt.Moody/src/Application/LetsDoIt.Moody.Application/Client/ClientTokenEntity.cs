using System;

namespace LetsDoIt.Moody.Application.Client
{
    public class ClientTokenEntity
    {
        public string Username { get; set; }

        public string Token { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
