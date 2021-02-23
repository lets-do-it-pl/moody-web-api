using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsDoIt.Moody.Web.Controllers
{
    using System;
    using Application.Constants;
    using Application.CustomExceptions;
    using Application.User;
    using LetsDoIt.Moody.Persistence.Entities;
    using Entities;
    using Entities.Requests.Account;
    using Entities.Responses;




    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _accountService;

        public AccountController(IUserService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [Authorize(Roles = RoleConstants.StandardRole)]
        public async Task<IActionResult> GetMyProfile()
        {
            try
            {
                var result = await _accountService.GetUserAsync(GetUserInfo().UserId);

                return Ok(ToAccountResponse(result));
            }
            catch (UserNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = RoleConstants.StandardRole)]
        public async Task<IActionResult> UpdateUser(AccountUpdateRequest accountUpdateRequest)
        {
            try
            {
                await _accountService.UpdateAccountDetails(GetUserInfo().UserId,
                    accountUpdateRequest.FullName,
                    accountUpdateRequest.Email,
                    accountUpdateRequest.Image);
            }
            catch (UserNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (MissingUserTypeException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        private AccountResponse ToAccountResponse(User user) =>
            new AccountResponse()
            {
                FullName = user.FullName,
                Image = user.Image == null ? null : Convert.ToBase64String(user.Image),
                Email = user.Email
            };

        private UserInfo GetUserInfo()
        {
            var fullName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            return new UserInfo(userId, fullName);
        }

    }
}
