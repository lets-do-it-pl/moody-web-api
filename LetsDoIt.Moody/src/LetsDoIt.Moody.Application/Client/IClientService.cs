using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Client
{
    public interface IClientService
    {
        Task SaveClientAsync(string userName,string password);

        Task<ClientTokenEntity> AuthenticateAsync(string username, string password);
    }
}
