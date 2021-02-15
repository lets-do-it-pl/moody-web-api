using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LetsDoIt.Moody.Web.Entities;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.Constants;
    using Application.Search;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = RoleConstants.StandardRole)]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        public async Task<IActionResult> SearchGeneral(string searchKey)
        {
            var value = await _searchService.GetGeneralSearchResultAsync(GetUserInfo().UserType, searchKey);

            return Ok(value);
        }

        private UserInfo GetUserInfo()
        {
            var fullName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var userType = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            return new UserInfo(userId, fullName, userType);
        }
    }
}
