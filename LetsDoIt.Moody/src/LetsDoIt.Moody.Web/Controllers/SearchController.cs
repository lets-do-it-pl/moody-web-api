using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.Constants;
    using Application.Search;
    using System;
    using System.Threading.Tasks;

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

        [HttpGet , Route("?")]//
        [AllowAnonymous]//herkese izin vermeli mi?
        public async Task<IActionResult> Authenticate(string searchKey)
        {
            try
            {
                var value = await _searchService.AutoCompleteSearch(searchKey);

                return Ok();
            }
            catch
            {
                throw;
            }

        }
    }
}
