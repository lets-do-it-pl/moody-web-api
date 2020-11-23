using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.Constants;
    using Application.User;
    using LetsDoIt.Moody.Infrastructure.ValueTypes;
    using Newtonsoft.Json;
    using System.Data;
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
