using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.Constants;
    using Application.User;

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
    }
}
