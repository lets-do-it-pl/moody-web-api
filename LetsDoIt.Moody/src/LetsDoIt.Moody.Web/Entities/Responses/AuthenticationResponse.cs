namespace LetsDoIt.Moody.Web.Entities.Responses
{
    public class AuthenticationResponse
    {
        public AuthenticationResponse(
            int userId, 
            string token)
        {
            UserId = userId;
            Token = token;
        }

        public int UserId { get; }

        public string Token { get; }
    }
}
