using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LetsDoIt.Moody.Web.Controllers
{
    using LetsDoIt.Moody.Application.Services;

   
    [Route("[controller]")]
    [ApiController]
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
            return new string[] { "Welcome", "Welcome" };
        }

        [AllowAnonymous]     
        [HttpPost("authenticate")]
        public IActionResult Authenticate(string username, string password)
        {
            var login = userService.EncryptUserNameAndPassword(username, password);
            var token = userService.Authenticate(login);
            if(token == null)
                return Unauthorized();
            return Ok(token);
        }
    }
}
