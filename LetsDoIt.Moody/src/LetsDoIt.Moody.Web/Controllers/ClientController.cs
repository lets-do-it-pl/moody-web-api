using System.Net;
using System.Threading.Tasks;
using System.Data;
using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.Client;
    using Entities.Requests;
    using Filters;

    [ApiController]
    [Route("api/client")]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService,
            ILogger<ClientController> logger)
        {
            _clientService = clientService;
            _logger = logger;
        }

        [HttpPost]
        [AuthorizationByTempToken]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> SaveClient(SaveClientRequest saveRequest)
        {
            _logger.LogInformation(
                $"{nameof(SaveClient)} is started with " +
                $"save request = {JsonConvert.SerializeObject(saveRequest)}");

            try
            {
                await _clientService.SaveClientAsync(
                                saveRequest.Username,
                                saveRequest.Password);

                _logger.LogInformation($"{nameof(SaveClient)} is finished successfully");

                return StatusCode((int)HttpStatusCode.Created, "Created");
            }
            catch (DuplicateNameException ex)
            {
                _logger.LogInformation($"{nameof(SaveClient)} is finished with bad request!");

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<ClientTokenEntity>> Authenticate(string username, string password)
        {
            _logger.LogInformation(
                $"{nameof(Authenticate)} is started with " +
                $"username={username}," +
                $"password={password}");

            try
            {
                var token = await _clientService.AuthenticateAsync(username, password);

                _logger.LogInformation($"{nameof(Authenticate)} is finished successfully");

                return Ok(token);
            }
            catch (AuthenticationException)
            {
                _logger.LogInformation($"{nameof(Authenticate)} is finished with Bad Request!");

                return BadRequest("Username or Password is wrong!");
            }
        }
    }
}