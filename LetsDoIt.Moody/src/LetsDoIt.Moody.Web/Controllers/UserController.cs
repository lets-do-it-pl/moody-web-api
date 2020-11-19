using System.Data;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using LetsDoIt.Moody.Application.CustomExceptions;
using LetsDoIt.Moody.Application.User;
using LetsDoIt.Moody.Web.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LetsDoIt.Moody.Web.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> SaveUser()
        {
            _logger.LogInformation(
                $"{nameof(SaveUser)} is started with " +
                $"save request = JsonConvert.SerializeObject()");

            try
            {
                await _userService.SaveUserAsync();

                _logger.LogInformation($"{nameof(SaveUser)} is finished successfully");

                return StatusCode((int)HttpStatusCode.Created, "Created");

            }
            catch (DuplicateNameException ex)
            {
                _logger.LogInformation($"{nameof(SaveUser)} is finished with bad request!");

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("email")]
        public async Task<IActionResult> SendEmailToken(string email)
        {
            _logger.LogInformation(
                $"{nameof(SendEmailToken)} is started with " +
                $"email = {email}");

            string referer = Request.Headers["Referer"].ToString();

            try
            {
                await _userService.SendSignUpEmailAsync(referer,email);

                _logger.LogInformation($"{nameof(SendEmailToken)} is finished successfully");

                return Ok();
            }
            catch (EmailNotRegisteredException exception )
            {
                _logger.LogInformation($"{nameof(SendEmailToken)} is finished with bad request!");

                return BadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("email/verification")]
        public async Task<IActionResult> ActivateUser( )
        {
            _logger.LogInformation(
                $"{nameof(ActivateUser)} is started with " +
                $"save request = ");

            try
            {
                await _userService.ActiveUserAsync(GetUserInfo().UserId);

                _logger.LogInformation($"{nameof(ActivateUser)} is finished successfully");

                return Ok();
            }
            catch (AuthenticationException exception)
            {
                _logger.LogInformation($"{nameof(ActivateUser)} is finished with bad request!");

                return BadRequest(exception.Message);
            }
            catch (TokenExpiredException exception)
            {
                _logger.LogInformation($"{nameof(ActivateUser)} is finished with bad request!");


                return BadRequest(exception.Message);
            }

        }

        private UserInfo GetUserInfo()
        {
            var fullName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            return new UserInfo(userId, fullName);
        }
    }
}
