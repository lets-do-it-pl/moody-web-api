using LetsDoIt.Moody.Application.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.User;
    using Entities.Requests;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System.Security.Authentication;

    [ApiController]
    [Route("api/users")]
    public class SystemUserController : ControllerBase
    {
        private readonly ILogger<SystemUserController> _logger;
        private readonly IUserService _userService;


        public SystemUserController(IUserService userService,
            ILogger<SystemUserController> logger)
        {
            _userService = userService;
            _logger = logger;
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
            catch (Exception)
            {
                throw;
            }

        }
    }
}
