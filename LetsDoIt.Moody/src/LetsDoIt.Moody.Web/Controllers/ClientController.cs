<<<<<<<< HEAD:LetsDoIt.Moody/src/Web/LetsDoIt.Moody.Web/Controllers/UserController.cs
﻿using System;
========
﻿using System.Net;
using System.Threading.Tasks;
>>>>>>>> master:LetsDoIt.Moody/src/LetsDoIt.Moody.Web/Controllers/ClientController.cs
using System.Data;
using System.Net;
using System.Security.Authentication;
<<<<<<<< HEAD:LetsDoIt.Moody/src/Web/LetsDoIt.Moody.Web/Controllers/UserController.cs
using System.Threading.Tasks;
using LetsDoIt.Moody.Application.CustomExceptions;
using LetsDoIt.Moody.Application.User;
using LetsDoIt.Moody.Domain;
using LetsDoIt.Moody.Infrastructure.ValueTypes;
========
>>>>>>>> master:LetsDoIt.Moody/src/LetsDoIt.Moody.Web/Controllers/ClientController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LetsDoIt.Moody.Web.Controllers
{
<<<<<<<< HEAD:LetsDoIt.Moody/src/Web/LetsDoIt.Moody.Web/Controllers/UserController.cs
    using Entities.Requests;
    using Entities.Responses;
    using System.Collections.Generic;
========
    using Application.Client;
    using Entities.Requests;
    using Filters;
>>>>>>>> master:LetsDoIt.Moody/src/LetsDoIt.Moody.Web/Controllers/ClientController.cs

    [ApiController]
    [Route("api/client")]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IClientService _clientService;

<<<<<<<< HEAD:LetsDoIt.Moody/src/Web/LetsDoIt.Moody.Web/Controllers/UserController.cs
        public UserController(IUserService userService, ILogger<UserController> logger)
========
        public ClientController(IClientService clientService,
            ILogger<ClientController> logger)
>>>>>>>> master:LetsDoIt.Moody/src/LetsDoIt.Moody.Web/Controllers/ClientController.cs
        {
            _clientService = clientService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
<<<<<<<< HEAD:LetsDoIt.Moody/src/Web/LetsDoIt.Moody.Web/Controllers/UserController.cs
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
========
        public async Task<IActionResult> SaveClient(SaveClientRequest saveRequest)
        {
            _logger.LogInformation(
                $"{nameof(SaveClient)} is started with " +
                $"save request = {JsonConvert.SerializeObject(saveRequest)}");

            try
            {
                await _clientService.SaveClientAsync(
                                saveRequest.Username,
                                saveRequest.Password);
>>>>>>>> master:LetsDoIt.Moody/src/LetsDoIt.Moody.Web/Controllers/ClientController.cs

                _logger.LogInformation($"{nameof(SaveClient)} is finished successfully");

                return StatusCode((int)HttpStatusCode.Created, "Created");
            }
            catch (DuplicateNameException ex)
            {
                _logger.LogInformation($"{nameof(SaveClient)} is finished with bad request!");

                return BadRequest(ex.Message);
            }
<<<<<<<< HEAD:LetsDoIt.Moody/src/Web/LetsDoIt.Moody.Web/Controllers/UserController.cs
        }

        [HttpPost]
        [Route("email")]
        public async Task<IActionResult> SendEmailToken(string email)
        {
            _logger.LogInformation(
                $"{nameof(SendEmailToken)} is started with " +
                $"email = {email}");

            try
            {
                await _userService.SendEmailTokenAsync(email);

                _logger.LogInformation($"{nameof(SendEmailToken)} is finished successfully");

                return Ok();
            }
            catch (EmailNotRegisteredException exception)
            {
                _logger.LogInformation($"{nameof(SendEmailToken)} is finished with bad request!");

                return BadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("email/verification")]
        public async Task<IActionResult> VerifyUserEmailToken(string token)
========
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<ClientTokenEntity>> Authenticate(string username, string password)
>>>>>>>> master:LetsDoIt.Moody/src/LetsDoIt.Moody.Web/Controllers/ClientController.cs
        {
            _logger.LogInformation(
                $"{nameof(VerifyUserEmailToken)} is started with " +
                $"save request = {token}");

            try
            {
<<<<<<<< HEAD:LetsDoIt.Moody/src/Web/LetsDoIt.Moody.Web/Controllers/UserController.cs
                await _userService.VerifyEmailTokenAsync(token);
========
                var token = await _clientService.AuthenticateAsync(username, password);
>>>>>>>> master:LetsDoIt.Moody/src/LetsDoIt.Moody.Web/Controllers/ClientController.cs

                _logger.LogInformation($"{nameof(VerifyUserEmailToken)} is finished successfully");

                return Ok();
            }
            catch (AuthenticationException exception)
            {
                _logger.LogInformation($"{nameof(VerifyUserEmailToken)} is finished with bad request!");

                return BadRequest(exception.Message);
            }
<<<<<<<< HEAD:LetsDoIt.Moody/src/Web/LetsDoIt.Moody.Web/Controllers/UserController.cs
            catch (TokenExpiredException exception)
            {
                _logger.LogInformation($"{nameof(VerifyUserEmailToken)} is finished with bad request!");


                return BadRequest(exception.Message);
            }

        }

        [Microsoft.AspNetCore.Cors.EnableCors("AnotherPolicy")]
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

            return Ok(userResult); //Abla System user get result burada isimizi goruyor. ileride get users 'i baskasi cagirip baska bir deger beklerse
            // buradan donen degeri baska bir objeye atip doneriz, ancak simdi gerek yok gibi duruyor. tamm. simdi test zamani
        }

        [Microsoft.AspNetCore.Cors.EnableCors("AnotherPolicy")]
        [HttpGet("{id:int}/details")]
        public async Task<IActionResult> GetUserDetails(int id)
        {
            _logger.LogInformation($"{nameof(GetUserDetails)} is started with user id = {id}");

            var userResult = await _userService.GetSystemUserDetails(id);

            if (userResult == null)
            {
                return NoContent();
            }

            _logger.LogInformation($"{nameof(GetUserDetails)} is finished successfully");

            return Ok(userResult);
        }
    }
}
========
        }
    }
}
>>>>>>>> master:LetsDoIt.Moody/src/LetsDoIt.Moody.Web/Controllers/ClientController.cs
