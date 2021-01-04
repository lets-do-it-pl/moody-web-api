using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    }
}
