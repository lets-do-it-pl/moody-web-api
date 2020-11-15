using System;

namespace LetsDoIt.Moody.Application.Security
{
    public class TokenInfo
    {
        public string Token { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
