namespace LetsDoIt.Moody.Application.Security
{
    public interface ISecurityService
    {
        TokenInfo GenerateJwtToken(string id, string fullName, string role);
    }
}
