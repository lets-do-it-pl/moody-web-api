using System;
using System.Data;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;
using LetsDoIt.Moody.Application.CustomExceptions;
using LetsDoIt.Moody.Application.User;
using LetsDoIt.Moody.Domain;
using LetsDoIt.Moody.Infrastructure.ValueTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Entities.Requests;
    using Entities.Responses;
    using System.Collections.Generic;

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
                    UserType.Normal,
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


        [Microsoft.AspNetCore.Cors.EnableCors("AnotherPolicy")]
        [HttpGet("getUser")]
        public async Task<IActionResult> GetAllUsers(/*int id*/)
        {
            _logger.LogInformation($"{nameof(GetAllUsers)} is started");

            var userResult = await _userService.GetSystemUsers(/*id*/);

            if (userResult == null)
            {
                return NoContent();
            }

            _logger.LogInformation($"{nameof(GetAllUsers)} is finished successfully");

            return Ok(userResult); 
        }

        
        [HttpGet("getAuthenticatedUser")]
        public async Task<IActionResult> GetUser(string username, string password)
        {
            _logger.LogInformation($"{nameof(GetUser)} is started");

            var userResult = await _userService.GetAuthenticatedUser(username, password);

            if (userResult == null)
            {
                return NoContent();
            }

            _logger.LogInformation($"{nameof(GetUser)} is finished successfully");

            return Ok(userResult);
        }
    }
}
