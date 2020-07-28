using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LetsDoIt.Moody.Web.Controllers
{
    using LetsDoIt.Moody.Application.Services;
   
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Welcome to home page" };
        }

        [AllowAnonymous]     
        [HttpPost("authenticate")]
        public IActionResult Authenticate(string username, string password)
        {
            var encryptedPassword = userService.EncryptUserNameAndPassword(username, password);
            var token = userService.Authenticate(username, encryptedPassword);
            if(token == null)
                return Unauthorized();
            return Ok(token);
        }
    }
}
