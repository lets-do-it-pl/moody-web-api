using System;
using System.Net;
using System.Threading.Tasks;
using System.Data;
using System.Security.Authentication;
using LetsDoIt.Moody.Application.CustomExceptions;
using LetsDoIt.Moody.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.User;
    using Entities.Requests;
    using Newtonsoft.Json;

    [ApiController]
    [Route("api/clients")]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IUserService _userService;

        public ClientController(IUserService userService,
            ILogger<ClientController> logger)
        {
            _userService = userService;
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
                await _userService.SaveUserAsync(
                                saveRequest.Username,
                                saveRequest.Password,
                                saveRequest.IsActive);

                _logger.LogInformation($"{nameof(SaveClient)} is finished successfully");

                return StatusCode((int)HttpStatusCode.Created, "Created");

            }
            catch (DuplicateNameException ex)
            {
                _logger.LogInformation($"{nameof(SaveClient)} is finished with bad request!");

                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<UserTokenEntity>> Authenticate(string username, string password)
        {
            _logger.LogInformation(
                $"{nameof(Authenticate)} is started with " +
                $"username={username}," +
                $"password={password}");

            try
            {
                var token = await _userService.AuthenticateAsync(username, password);

                _logger.LogInformation($"{nameof(Authenticate)} is finished successfully");

                return Ok(token);
            }
            catch (AuthenticationException)
            {
                _logger.LogInformation($"{nameof(Authenticate)} is finished with Bad Request!");

                return BadRequest("Username or Password is wrong!");
            }

            catch (UserNotActiveException exception)
            {
                _logger.LogInformation($"{nameof(Authenticate)} is finished with Bad Request!");


                //////REDIRECT TO ACTIVATION.

                return BadRequest(exception.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}


