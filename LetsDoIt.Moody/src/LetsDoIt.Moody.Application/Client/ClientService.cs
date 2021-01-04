using System.Data;
using System.Security.Authentication;
using System.Threading.Tasks;
using NGuard;

namespace LetsDoIt.Moody.Application.Client
{
    using Constants;
    using Infrastructure.Utils;
    using Persistence.Entities;
    using Persistence.Repositories.Base;
    using Security;

    public class ClientService : IClientService
    {
        private readonly IRepository<Client> _clientRepository;
        private readonly ISecurityService _securityService;

        public ClientService(
            IRepository<Client> clientRepository,
            ISecurityService securityService)
        {
            _clientRepository = clientRepository;
            _securityService = securityService;
        }

        public async Task SaveClientAsync(string username, string password)
        {
            Guard.Requires(username, nameof(username)).IsNotNullOrEmptyOrWhiteSpace();
            Guard.Requires(password, nameof(password)).IsNotNullOrEmptyOrWhiteSpace();

            var isUserExisted = await _clientRepository.AnyAsync(u => u.Username == username);
            if (isUserExisted)
            {
                throw new DuplicateNameException($"The username of the client is already in the database. Username = {username}");
            }

            var client = ToClientEntity(username, password);

            await _clientRepository.AddAsync(new Client
            {
                Username = client.Username,
                Password = client.EncryptedPassword
            });

        }

        public async Task<ClientTokenEntity> AuthenticateAsync(string username, string password)
        {
            Guard.Requires(username, nameof(username)).IsNotNullOrEmptyOrWhiteSpace();
            Guard.Requires(password, nameof(password)).IsNotNullOrEmptyOrWhiteSpace();

            var clientEntity = ToClientEntity(username, password);

            var clientDb = await _clientRepository
                                    .GetAsync(u =>
                                        u.Username == clientEntity.Username &&
                                        u.Password == clientEntity.EncryptedPassword);
            if (clientDb == null)
            {
                throw new AuthenticationException();
            }

            var tokenInfo = _securityService.GenerateJwtToken(clientDb.Id.ToString(), clientDb.Username, UserTypeConstants.Client);

            return new ClientTokenEntity(
                        username,
                        tokenInfo.Token,
                        tokenInfo.ExpirationDate);
        }

        private static ClientEntity ToClientEntity(string username, string password) =>
            new ClientEntity
            (
                username,
                ProtectionHelper.EncryptValue(username + password)
            );
    }
}
