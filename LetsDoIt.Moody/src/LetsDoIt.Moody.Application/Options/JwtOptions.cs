namespace LetsDoIt.Moody.Application.Options
{
    public class JwtOptions
    {
        public const string JwtSectionName = "Jwt";

        public string SecretKey { get; set; }

        public int TokenExpirationMinutes { get; set; }
    }
}
