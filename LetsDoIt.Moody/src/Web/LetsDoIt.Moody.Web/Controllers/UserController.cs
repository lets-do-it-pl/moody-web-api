using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetsDoIt.Moody.Application;
using LetsDoIt.Moody.Application.Services;
using LetsDoIt.Moody.Application.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace LetsDoIt.Moody.Web.Controllers
{
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
        public void SaveUser(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("Username cannot be null!");
            } 
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be null!");
            }

            _userService.SaveUser(userName,password);

        }
    }
}
