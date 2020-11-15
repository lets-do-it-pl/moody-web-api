namespace LetsDoIt.Moody.Application.Security
{
    public interface ISecurityService
    {
        TokenInfo GetNewToken(string id, string role);

        bool ValidateToken(string token);

        string GetClaim(string token, string claimType);
    }
}
