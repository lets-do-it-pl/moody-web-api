using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.User;

    [ApiController]
    [Route("api/users")]
    public class UserController:ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost]
        public async  Task  SaveUser(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("Username cannot be null!");
            } 
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be null!");
            }
            
            await _userService.SaveUserAsync(userName,password);

        }
    }
}   
