using System;

namespace LetsDoIt.Moody.Application.User
{
    public class UserTokenEntity
    {
        public string Username { get; set; }

        public string Token { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
