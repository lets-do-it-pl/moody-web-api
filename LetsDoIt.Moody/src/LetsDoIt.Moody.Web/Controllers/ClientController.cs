using System.Net;
using System.Threading.Tasks;
using System.Data;
using System.Security.Authentication;
using LetsDoIt.Moody.Web.Entities.Requests.Client;
using Microsoft.AspNetCore.Mvc;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.Client;
    using Entities.Requests;
    using Filters;

    [ApiController]
    [Route("api/client")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost]
        [AuthorizationByTempToken]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> SaveClient(SaveClientRequest saveRequest)
        {
            try
            {
                await _clientService.SaveClientAsync(
                                saveRequest.Username,
                                saveRequest.Password);

                return StatusCode((int)HttpStatusCode.Created, "Created");
            }
            catch (DuplicateNameException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<ClientTokenEntity>> Authenticate(string username, string password)
        {
            try
            {
                var token = await _clientService.AuthenticateAsync(username, password);

                return Ok(token);
            }
            catch (AuthenticationException)
            {
                return BadRequest("Username or Password is wrong!");
            }
        }
    }
}