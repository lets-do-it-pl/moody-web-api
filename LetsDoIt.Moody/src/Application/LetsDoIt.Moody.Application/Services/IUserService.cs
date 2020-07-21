using System;
namespace LetsDoIt.Moody.Application.Services
{
    public interface IUserService
    {
        string Encrypt(string username, string password);
        string Authenticate(string username, string password);
       
    }
}
