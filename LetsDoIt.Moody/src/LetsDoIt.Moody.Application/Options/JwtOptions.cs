namespace LetsDoIt.Moody.Application.Options
{
    public class JwtOptions
    {
        public const string Jwt = "Jwt";

        public string SecretKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int TokenExpirationMinutes { get; set; }
    }
}
