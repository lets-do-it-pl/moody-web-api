using System;
using System.Data;
using System.Net;
using System.Threading.Tasks;
using LetsDoIt.Moody.Application.User;
using LetsDoIt.Moody.Web.Entities.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LetsDoIt.Moody.Web.Controllers
{
    [ApiController]
    [Route("api/users/system-user")]
    public class SystemUserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public SystemUserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> SaveSystemUser(SaveSystemUserRequest saveSystemUserRequest)
        {
            _logger.LogInformation(
                $"{nameof(SaveSystemUser)} is started with " +
                $"save request = {JsonConvert.SerializeObject(saveSystemUserRequest)}");

            try
            {
                await _userService.SaveUserAsync(
                    saveSystemUserRequest.Username,
                    saveSystemUserRequest.Password);

                _logger.LogInformation($"{nameof(SaveSystemUser)} is finished successfully");

                return StatusCode((int)HttpStatusCode.Created, "Created");

            }
            catch (DuplicateNameException ex)
            {
                _logger.LogInformation($"{nameof(SaveSystemUser)} is finished with bad request!");

                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
