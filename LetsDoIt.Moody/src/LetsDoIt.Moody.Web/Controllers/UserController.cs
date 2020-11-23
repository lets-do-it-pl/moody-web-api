using System.Data;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using LetsDoIt.Moody.Application.CustomExceptions;
using LetsDoIt.Moody.Web.Entities;
using LetsDoIt.Moody.Web.Entities.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.Constants;
    using Application.User;

    [Route("api/user")]
    [ApiController]
    [Authorize(Roles = RoleConstants.StandardRole)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IUserService userService,
            ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [AllowAnonymous]
        public async Task<IActionResult> SaveUser(SaveUserRequest saveUserRequest)
        {
            _logger.LogInformation(
                $"{nameof(SaveUser)} is started with " +
                $"save request = {JsonConvert.SerializeObject(saveUserRequest)}");

            try
            {
                await _userService.SaveUserAsync(saveUserRequest.Username,
                    saveUserRequest.Password,
                    saveUserRequest.Email,
                    saveUserRequest.Name,
                    saveUserRequest.Surname);

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
        [Route("user-verification-email")]
        [AllowAnonymous]
        public async Task<IActionResult> SendUserVerificationEmail(string email)
        {
            _logger.LogInformation(
                $"{nameof(SendUserVerificationEmail)} is started with " +
                $"email = {email}");

            var referer = Request.Headers["Referer"].ToString();

            try
            {
                await _userService.SendActivationEmailAsync(referer, email);

                _logger.LogInformation($"{nameof(SendUserVerificationEmail)} is finished successfully");

                return Ok();
            }
            catch (EmailNotRegisteredException exception)
            {
                _logger.LogInformation($"{nameof(SendUserVerificationEmail)} is finished with bad request!");

                return BadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("email/verification")]
        public async Task<IActionResult> ActivateUser()
        {
            _logger.LogInformation(
                $"{nameof(ActivateUser)} is started");

            try
            {
                await _userService.ActivateUser(GetUserInfo().UserId);
                _logger.LogInformation($"{nameof(ActivateUser)} is finished successfully");
                return Ok();
            }
            catch (UserNotFoundException e)
            {
                _logger.LogInformation($"{nameof(ActivateUser)} is finished with bad request!");

                return BadRequest(e.Message);
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
