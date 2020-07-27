using System;
namespace LetsDoIt.Moody.Application.Services
{
    public interface IUserService
    {
        string EncryptUserNameAndPassword(string username, string password);
        string Authenticate(string login);
       
    }
}
