using System.Data;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NGuard;

namespace LetsDoIt.Moody.Application.Client
{
    using Constants;
    using Domain;
    using Infrastructure.Utils;
    using Persistance.Repositories.Base;
    using Security;

    public class ClientService : IClientService
    {
        private readonly IEntityRepository<Client> _clientRepository;
        private readonly ISecurityService _securityService;
        private readonly ILogger<ClientService> _logger;

        public ClientService(
            IEntityRepository<Client> clientRepository,
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

            var isUserExisted = await _clientRepository.AnyAsync(u => u.UserName == username && !u.IsDeleted);
            if (isUserExisted)
            {
                throw new DuplicateNameException($"The username of the client is already in the database. Username = {username}");
            }

            var client = ToClient(username, password);

            await _clientRepository.AddAsync(new Client
            {
                UserName = client.Username,
                Password = client.EncryptedPassword
            });
            
            _logger.LogInformation($"{nameof(SaveClientAsync)} executed with username={username}.");
        }

        public async Task<ClientTokenEntity> AuthenticateAsync(string username, string password)
        {
            Guard.Requires(username, nameof(username)).IsNotNullOrEmptyOrWhiteSpace();
            Guard.Requires(password, nameof(password)).IsNotNullOrEmptyOrWhiteSpace();

            _logger.LogInformation($"{nameof(AuthenticateAsync)} executing with username={username}...");

            var user = ToClient(username, password);

            var userDb = await _clientRepository
                                    .GetAsync(u =>
                                        u.UserName == user.Username &&
                                        u.Password == user.EncryptedPassword &&
                                        !u.IsDeleted);
            if (userDb == null)
            {
                throw new AuthenticationException();
            }

            var tokenInfo = _securityService.GetNewToken(userDb.Id.ToString(), UserTypeConstants.Client);
            
            _logger.LogInformation($"{nameof(AuthenticateAsync)} executed with username={username}.");

            return new ClientTokenEntity
            {
                Username = username,
                Token = tokenInfo.Token,
                ExpirationDate = tokenInfo.ExpirationDate
            };
        }

        private static ClientEntity ToClient(string username, string password) =>
            new ClientEntity
            (
                username,
                ProtectionHelper.EncryptValue(username + password)
            );
    }
}
