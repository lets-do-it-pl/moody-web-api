using System;
namespace LetsDoIt.Moody.Application.Services
{
    public interface IUserService
    {
        string EncryptUserNameAndPassword();
        string Authenticate(string username, string password);
       
    }
}
