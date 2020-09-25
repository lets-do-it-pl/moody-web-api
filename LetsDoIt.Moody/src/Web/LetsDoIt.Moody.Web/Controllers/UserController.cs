using System;
using System.Net;
using System.Threading.Tasks;
using System.Data;
using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.User;
    using Entities.Requests;

    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(IUserService userService,
            ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        [AuthorizationByTempToken]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> SaveUser(SaveUserRequest saveRequest)
        {
            _logger.LogInformation($"{nameof(SaveUser)} is started with save request = {saveRequest}");
            try
            {
                await _userService.SaveUserAsync(
                                saveRequest.Username,
                                saveRequest.Password);

                return StatusCode((int)HttpStatusCode.Created, "Created");

            }
            catch (DuplicateNameException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
            _logger.LogInformation($"{nameof(SaveUser)} is finished successfully");
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<UserTokenEntity>> Authenticate(string username, string password)
        {
            _logger.LogInformation($"{nameof(Authenticate)} is started with username and  password = {username},{password}");
            try
            {
                var token = await _userService.AuthenticateAsync(username, password);

                return Ok(token);
            }
            catch (AuthenticationException)
            {
                return BadRequest("Username or Password is wrong!");
            }
            catch (Exception)
            {
                throw;
            }
            _logger.LogInformation($"{nameof(Authenticate)} is finished successfully");
        }
    }
}


