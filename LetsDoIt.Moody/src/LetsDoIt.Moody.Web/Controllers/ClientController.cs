using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data;
using System.Net;
using System.Threading.Tasks;

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
    }
}
