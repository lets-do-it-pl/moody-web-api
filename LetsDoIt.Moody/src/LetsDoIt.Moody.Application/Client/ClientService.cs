using System.Data;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ClientService> _logger;

        public ClientService(
            IRepository<Client> clientRepository,
            ISecurityService securityService,
            ILogger<ClientService> logger)
        {
            _clientRepository = clientRepository;
            _securityService = securityService;
            _logger = logger;
        }

        public async Task SaveClientAsync(string username, string password)
        {
            Guard.Requires(username, nameof(username)).IsNotNullOrEmptyOrWhiteSpace();
            Guard.Requires(password, nameof(password)).IsNotNullOrEmptyOrWhiteSpace();

            _logger.LogInformation($"{nameof(SaveClientAsync)} executing with username={username}...");

            var isUserExisted = await _clientRepository.AnyAsync(u => u.UserName == username);
            if (isUserExisted)
            {
                throw new DuplicateNameException($"The username of the client is already in the database. Username = {username}");
            }

            var client = ToClientEntity(username, password);

            await _clientRepository.AddAsync(new Client
            {
                UserName = client.Username,
                Password = client.EncryptedPassword
            });

            _logger.LogInformation($"{nameof(SaveClientAsync)} executed with username={username}.");
        }

        public async Task<Client> GetClient(int id)
        {
            var result = await _clientRepository.SingleOrDefaultAsync(c => c.Id == id);

            return new Client
            {
                Id = result.Id,
                UserName = result.UserName
            };
        }

        public async Task<ClientTokenEntity> AuthenticateAsync(string username, string password)
        {
            Guard.Requires(username, nameof(username)).IsNotNullOrEmptyOrWhiteSpace();
            Guard.Requires(password, nameof(password)).IsNotNullOrEmptyOrWhiteSpace();

            _logger.LogInformation($"{nameof(AuthenticateAsync)} executing with username={username}...");

            var user = ToClientEntity(username, password);

            var userDb = await _clientRepository
                                    .GetAsync(u =>
                                        u.UserName == user.Username &&
                                        u.Password == user.EncryptedPassword);
            if (userDb == null)
            {
                throw new AuthenticationException();
            }

            var tokenInfo = _securityService.GenerateJwtToken(userDb.Id.ToString(), userDb.UserName, UserTypeConstants.Client);

            _logger.LogInformation($"{nameof(AuthenticateAsync)} executed with username={username}.");

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
