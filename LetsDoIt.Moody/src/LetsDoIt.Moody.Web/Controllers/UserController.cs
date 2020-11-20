using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using System.Data;
using System.Security.Authentication;
using Newtonsoft.Json;


namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.Constants;
    using Application.CustomExceptions;
    using Application.User;
    using Entities.Requests;
    using Entities.Responses;
    using System.Collections.Generic;
    using Application.Client;
    using Entities.Requests;
    using Filters;

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


        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            _logger.LogInformation($"{nameof(GetUsers)} is started");

            var userResult = await _userService.GetSystemUsers();

            if (userResult == null)
            {
                return NoContent();
            }
            _logger.LogInformation($"{nameof(GetUsers)} is finished successfully");

            return Ok(userResult);

        }

        [HttpGet("{id:int}/details")]
        public async Task<IActionResult> GetUserDetails(int id)
        {
            _logger.LogInformation($"{nameof(GetUserDetails)} is started");

            var userDetailsResult = await _userService.GetSystemUserDetails(id);

            if (userDetailsResult == null)
            {
                return NoContent();
            }
            _logger.LogInformation($"{nameof(GetUserDetails)} is finished successfully");

            return Ok(userDetailsResult);

        }



    }
}
