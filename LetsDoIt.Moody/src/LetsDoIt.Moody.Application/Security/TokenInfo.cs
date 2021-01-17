using System;

namespace LetsDoIt.Moody.Application.Security
{
    public class TokenInfo
    {
        public TokenInfo(
            string token, 
            DateTime expirationDate)
        {
            Token = token;
            ExpirationDate = expirationDate;
        }

        public string Token { get; }

        public DateTime ExpirationDate { get; }
    }
}
