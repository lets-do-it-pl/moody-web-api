using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Authentication;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.User;
    using Web.Entities.Requests;

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
        public async Task<IActionResult> SaveUser(SaveUserRequest saveRequest)
        {
            try
            {
                await _userService.SaveUserAsync(
                                saveRequest.Username,
                                saveRequest.Password);

                return Ok();
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
