using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LetsDoIt.Moody.Web.Controllers
{
    using LetsDoIt.Moody.Application.Services;

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        // GET: api/User
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Welcome" };
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

      [AllowAnonymous]     
      [HttpPost("authenticate")]
      public IActionResult Authenticate([FromBody] UserCred userCred)
        {
            var token = userService.Authenticate(userCred.Username, userCred.Password);
            if(token == null)
                return Unauthorized();
            return Ok(token);
        }
    }
}
