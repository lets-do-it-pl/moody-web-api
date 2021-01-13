using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.Constants;
    using Application.Search;
    using System;
    using System.Threading.Tasks;

    [Route("api/search")]
    [ApiController]
    [Authorize(Roles = RoleConstants.StandardRole)]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet , Route("/{searchKey}")]
        
        public async Task<IActionResult> GeneralSearch(string searchKey)
        {

            searchKey.ToLower();

            if(searchKey == null)
            {
                Console.WriteLine("Please write sth!!");

            }

            var value = await _searchService.DataSearchAsync(searchKey);

            return Ok(value);
            
        }
    }
}
