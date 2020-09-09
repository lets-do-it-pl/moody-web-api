using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.User;
    using LetsDoIt.Moody.Web.Entities.Requests;
    using Microsoft.Extensions.Logging;
    using System.Data;
    using System.Security.Authentication;
    using Web.Entities.Requests;

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
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> SaveUser(SaveUserRequest saveRequest)
        {
            _logger.LogInformation($"{SaveUser(saveRequest)} is started with save request = {saveRequest}");
            try
            {
                await _userService.SaveUserAsync(
                                saveRequest.Username,
                                saveRequest.Password);

                

                return StatusCode((int)HttpStatusCode.Created,"Created");

            }
            catch (DuplicateNameException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
            _logger.LogInformation($"{SaveUser(saveRequest)} is finished successfully");
        }


        [HttpPost("authenticate")]
        public async Task<ActionResult<UserTokenEntity>> Authenticate(string username, string password)
        {
            _logger.LogInformation($"{Authenticate(username,password)} is started with user name and password  = {username}{password}");
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
            _logger.LogInformation($"{Authenticate(username, password)} is finished successfully");
        }
    }
}
