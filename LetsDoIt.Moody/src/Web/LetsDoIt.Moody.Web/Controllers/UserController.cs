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

    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> SaveUser(SaveUserRequest saveRequest)
        {
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
        }

        public Exception ArgumentOutOfRangeException { get; set; }

        [HttpPost("authenticate")]
        public async Task<ActionResult<UserTokenEntity>> Authenticate(string username, string password)
        {
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
        }
    }
}
