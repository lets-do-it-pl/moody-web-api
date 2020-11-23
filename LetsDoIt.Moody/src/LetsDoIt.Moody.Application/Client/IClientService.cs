using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Client
{
    using LetsDoIt.Moody.Persistence.Entities;
    public interface IClientService
    {
        Task SaveClientAsync(string userName,string password);

        Task<ClientTokenEntity> AuthenticateAsync(string username, string password);

        Task<Client> GetClient(int id);
    }
}
