using System.Data;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.CustomExceptions;
    using Entities;
    using Entities.Requests;
    using Application.Constants;
    using Application.User;
    using Entities.Responses;
    using System;
    using LetsDoIt.CustomValueTypes;

    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate(AuthenticationRequest request)
        {
            try
            {
                var value = await _userService.AuthenticateAsync(request.Email, request.Password);

                var result = new AuthenticationResponse(value.id, value.token);

                return Ok(result);
            }
            catch (UserNotFoundException)
            {
                return BadRequest("Username or Password is wrong!");
            }
            catch (Exception ex)
            {
                if (ex is UserNotActiveException || ex is UserNotHaveLoginPermissionException)
                {
                    return BadRequest(ex.Message);
                }

                throw;
            }

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
            catch (Exception ex)
            {
                if (ex is DuplicateNameException)
                {
                    return BadRequest(ex.Message);
                }
                else if (ex is UserNotRegisteredException || ex is UserAlreadyActivatedException)
                {
                    return BadRequest($"The user has been created! Error on sending activation email: {ex.Message}");
                }

                throw;
            }
        }

        [HttpPost("user-verification-email")]        
        [AllowAnonymous]
        public async Task<IActionResult> SendUserVerificationEmail(string email)
        {
            try
            {
                await _userService.SendActivationEmailAsync(email);

                return Ok();
            }
            catch (UserNotRegisteredException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("activate")]
        [Authorize(Roles = RoleConstants.NotActivatedUserRole)]
        public async Task<IActionResult> ActivateUser()
        {
            try
            {
                await _userService.ActivateUserAsync(GetUserInfo().UserId);

                return Ok();
            }
            catch (Exception ex)
            {
                if (ex is UserNotFoundException || ex is UserAlreadyActivatedException)
                {
                    return BadRequest(ex.Message);
                }

                throw;
            }
        }

        [HttpPost("forget-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordRequest request)
        {
            try
            {
                await _userService.ForgetPasswordAsync(request.Email);

                return Ok();
            }
            catch (Exception ex)
            {
                if (ex is UserNotFoundException || 
                    ex is UserNotActiveException ||
                    ex is UserNotHaveLoginPermissionException)
                {
                    return BadRequest(ex.Message);
                }

                throw;
            }
        }

        [HttpPost("reset-password")]
        [Authorize(Roles = RoleConstants.ResetPasswordRole)]
        public async Task<IActionResult> ResetPassword(string password)
        {
            try
            {
                await _userService.ResetPasswordAsync(GetUserInfo().UserId, password);

                return Ok();
            }
            catch (Exception ex)
            {
                if (ex is UserNotFoundException ||
                    ex is UserNotActiveException ||
                    ex is UserNotHaveLoginPermissionException)
                {
                    return BadRequest(ex.Message);
                }

                throw;
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
