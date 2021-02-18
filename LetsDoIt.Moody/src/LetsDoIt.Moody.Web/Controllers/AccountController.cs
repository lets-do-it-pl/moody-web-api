using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LetsDoIt.Moody.Application.Account;
using LetsDoIt.Moody.Application.Constants;
using LetsDoIt.Moody.Application.CustomExceptions;
using LetsDoIt.Moody.Persistence.Entities;
using LetsDoIt.Moody.Web.Entities;
using LetsDoIt.Moody.Web.Entities.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsDoIt.Moody.Web.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [Authorize(Roles = RoleConstants.AdminRole)]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var result = await _accountService.GetAccount(GetUserInfo().UserId);

                return Ok(ToAccountResponse(result));
            }
            catch (UserNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }
        private UserInfo GetUserInfo()
        {
            var fullName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            return new UserInfo(userId, fullName);
        }
        private AccountResponse ToAccountResponse(User user) =>
            new AccountResponse()
            {
                FullName = user.FullName,
                Image = user.Image,
                Email = user.Email
            };
    }
}
