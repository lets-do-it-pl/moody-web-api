using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.Constants;
    using Application.User;
    using LetsDoIt.Moody.Infrastructure.ValueTypes;
    using System.Net;
    using System.Threading.Tasks;

    [Route("api/user")]
    [ApiController]
    [Authorize(Roles = RoleConstants.AdminRole)]
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
        [ProducesResponseType((int)HttpStatusCode.Created)
        public async Task<IActionResult> SaveUser(SaveUserRequest saveUserRequest)
        {
            _logger.LogInformation(
                $"{nameof(SaveUser)} is started with " +
                $"save request = {JsonConvert.SerializeObject(saveUserRequest)}");

            try
            {
                await _userService.SaveUserAsync(
                    saveUserRequest.Username,
                    saveUserRequest.Password,
                    false,
                    UserType.Normal,
                    saveUserRequest.Name,
                    saveUserRequest.Surname,
                    Email.Parse(saveUserRequest.Email)
                    );

                _logger.LogInformation($"{nameof(SaveUser)} is finished successfully");

                return StatusCode((int)HttpStatusCode.Created, "Created");

            }
            catch (DuplicateNameException ex)
            {
                _logger.LogInformation($"{nameof(SaveUser)} is finished with bad request!");

                return BadRequest(ex.Message);
            }
        }

        [Microsoft.AspNetCore.Cors.EnableCors("AnotherPolicy")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            _logger.LogInformation($"{nameof(GetUser)} is started");

            var userResult = await _userService.GetUser(id);

            if (userResult == null)
            {
                return NoContent();
            }

            _logger.LogInformation($"{nameof(GetUser)} is finished successfully");

            return Ok(userResult);
        }
    }
}
