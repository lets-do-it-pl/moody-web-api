﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.Constants;
    using Application.CustomExceptions;
    using Application.User;
    using Entities;
    using Entities.Requests.User;
    using Entities.Responses;
    using LetsDoIt.Moody.Persistence.Entities;
    using System;

    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        #region UserCRUD

        [HttpGet]
        [Authorize(Roles = RoleConstants.AdminRole)]
        public async Task<IActionResult> GetUsers()
        {
            var userResult = await _userService.GetUsersAsync();

            if (userResult == null)
            {
                return NoContent();
            }

            var result = userResult
                .Select(ToUserResponse);

            return Ok(result);
        }
        
        [HttpGet, Route("{userId}")]
        [Authorize(Roles = RoleConstants.AdminRole)]
        public async Task<IActionResult> GetUser(int userId)
        {
            try
            {
                var result = await _userService.GetUserAsync(userId);

                return Ok(ToUserDetailsResponse(result));
            }
            catch (UserNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [AllowAnonymous]
        public async Task<IActionResult> SaveUser(SaveUserRequest saveUserRequest)
        {
            try
            {
                await _userService.SaveUserAsync(
                    saveUserRequest.Email,
                    saveUserRequest.Password,
                    saveUserRequest.FullName);

                return StatusCode((int)HttpStatusCode.Created, "Created");
            }
            catch (Exception ex)
            {
                if (ex is DuplicateNameException)
                {
                    return BadRequest(ex.Message);
                }

                if (ex is UserNotFoundException || ex is UserAlreadyActivatedException)
                {
                    return BadRequest($"The user has been created! Error on sending activation email: {ex.Message}");
                }

                throw;
            }
        }

        [HttpPut, Route("{userId}")]
        [Authorize(Roles = RoleConstants.AdminRole)]
        public async Task<IActionResult> UpdateUser(int userId, UserUpdateRequest userUpdateRequest)
        {
            try
            {
                await _userService.UpdateUserAsync(ToUserUpdateEntity(userId,userUpdateRequest));
            }
            catch (UserNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (MissingUserTypeException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpDelete, Route("{userId}")]
        [Authorize(Roles = RoleConstants.AdminRole)]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                await _userService.DeleteUserAsync(GetUserInfo().UserId,userId);
            }
            catch (UserNotFoundException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        #endregion

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate(UserAuthenticationRequest request)
        {
            try
            {
                var value = await _userService.AuthenticateAsync(request.Email, request.Password);

                var result = new AuthenticationResponse(value.id, value.token, value.fullName);

                return Ok(result);
            }
            catch (UserNotFoundException)
            {
                return BadRequest("Email or Password is wrong!");
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

        [HttpPost("user-verification-email")]
        [AllowAnonymous]
        public async Task<IActionResult> SendUserVerificationEmail(string email)
        {
            try
            {
                await _userService.SendActivationEmailAsync(email);

                return Ok();
            }
            catch (UserNotFoundException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("activate")]
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
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            try
            {
                await _userService.ResetPasswordAsync(GetUserInfo().UserId, request.Password);

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

        private UserResponse ToUserResponse(User user) =>
            new UserResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Description = user.Description,
                UserType = user.UserType,
                CreateDate = user.CreatedDate
            };  
        
        private UserDetailsResponse ToUserDetailsResponse(User user) =>
            new UserDetailsResponse()
            {
                Id = user.Id,
                CanLogin = user.CanLogin,
                Email = user.Email,
                FullName = user.FullName,
                UserType = user.UserType,
                IsActive = user.IsActive,
                Description = user.Description
            };
        
        private UserUpdateEntity ToUserUpdateEntity(int userId, UserUpdateRequest user) =>
            new UserUpdateEntity()
            {
                Id = userId,
                CanLogin = user.CanLogin,
                Email = user.Email,
                FullName = user.FullName,
                UserType = user.UserType,
                IsActive = user.IsActive,
                ModifiedById = GetUserInfo().UserId,
                Password = user.Password
            };
    }
}
