namespace LetsDoIt.Moody.Web.Entities.Responses
{
    public class AuthenticationResponse
    {
        public AuthenticationResponse(
            int userId,
            string token, 
            string fullName)
        {
            UserId = userId;
            Token = token;
            FullName = fullName;
        }

        public int UserId { get; }

        public string Token { get; }

        public string FullName { get; }
    }
}
