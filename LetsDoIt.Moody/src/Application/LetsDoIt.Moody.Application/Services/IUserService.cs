namespace LetsDoIt.Moody.Application.Services
{
    public interface IUserService
    {
        public string EncryptUserNameAndPassword(string username, string password);
        public string Authenticate(string username, string password);
       
    }
}
