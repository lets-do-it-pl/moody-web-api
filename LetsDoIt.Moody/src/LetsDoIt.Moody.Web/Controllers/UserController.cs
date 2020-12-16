using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Web.Controllers
{
    using LetsDoIt.Moody.Application.CustomExceptions;
    using LetsDoIt.Moody.Web.Entities;
    using LetsDoIt.Moody.Web.Entities.Requests;
    using Application.Constants;
    using Application.User;

    [Route("api/user")]
    [ApiController]
    [Authorize(Roles = RoleConstants.StandardRole)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [AllowAnonymous]
        public async Task<IActionResult> SaveUser(SaveUserRequest saveUserRequest)
        {

            try
            {
                await _userService.SaveUserAsync(saveUserRequest.Username,
                    saveUserRequest.Password,
                    saveUserRequest.Email,
                    saveUserRequest.Name,
                    saveUserRequest.Surname);

                return StatusCode((int)HttpStatusCode.Created, "Created");
            }
            catch (DuplicateNameException ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("user-verification-email")]
        [AllowAnonymous]
        public async Task<IActionResult> SendUserVerificationEmail(string email)
        {

            var referer = Request.Headers["Referer"].ToString();

            try
            {
                await _userService.SendActivationEmailAsync(referer, email);

                return Ok();
            }
            catch (EmailNotRegisteredException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("email/verification")]
        public async Task<IActionResult> ActivateUser()
        {
            try
            {
                await _userService.ActivateUser(GetUserInfo().UserId);

                return Ok();
            }
            catch (UserNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }

        private UserInfo GetUserInfo()
        {
            var fullName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            return new UserInfo(userId, fullName);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUser(int id)
        {
            var userResult = await _userService.GetUserAsync(id);

            return Ok(userResult);
        }

        [HttpPost("id/newpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword(int id, string newPassword)
        {
            var user = await _userService.GetUserAsync(id);

            await _userService.ChangePasswordAsync(user.Id, newPassword);

            return Ok(user.Username + "'s password is successfully changed");
        }
    }
}
