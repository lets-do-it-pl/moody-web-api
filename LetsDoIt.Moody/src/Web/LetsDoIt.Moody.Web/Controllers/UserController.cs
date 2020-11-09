﻿using System.Data;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;
using LetsDoIt.Moody.Application.CustomExceptions;
using LetsDoIt.Moody.Application.User;
using LetsDoIt.Moody.Domain;
using LetsDoIt.Moody.Web.Entities.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LetsDoIt.Moody.Web.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
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
                    UserType.Standard,
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

        [HttpPost]
        [Route("email")]
        public async Task<IActionResult> SendEmailToken(string email)
        {
            _logger.LogInformation(
                $"{nameof(SendEmailToken)} is started with " +
                $"email = {email}");

            string referer = Request.Headers["Referer"].ToString();

            try
            {
                await _userService.SendSignUpEmailAsync(referer,email);

                _logger.LogInformation($"{nameof(SendEmailToken)} is finished successfully");

                return Ok();
            }
            catch (EmailNotRegisteredException exception )
            {
                _logger.LogInformation($"{nameof(SendEmailToken)} is finished with bad request!");

                return BadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("email/verification")]
        public async Task<IActionResult> VerifyUserEmailToken(string token)
        {
            _logger.LogInformation(
                $"{nameof(VerifyUserEmailToken)} is started with " +
                $"save request = {token}");

            try
            {
                await _userService.VerifyEmailTokenAsync(token);

                _logger.LogInformation($"{nameof(VerifyUserEmailToken)} is finished successfully");

                return Ok();
            }
            catch (AuthenticationException exception)
            {
                _logger.LogInformation($"{nameof(VerifyUserEmailToken)} is finished with bad request!");

                return BadRequest(exception.Message);
            }
            catch (TokenExpiredException exception)
            {
                _logger.LogInformation($"{nameof(VerifyUserEmailToken)} is finished with bad request!");


                return BadRequest(exception.Message);
            }

        }
    }
}
