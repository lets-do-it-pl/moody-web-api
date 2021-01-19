using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.Constants;
    using Application.Dashboard;
    using LetsDoIt.Moody.Persistence.StoredProcedures.ResultEntities;
    using LetsDoIt.Moody.Web.Entities.Responses;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = RoleConstants.StandardRole)]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<ActionResult<IEnumerable<DashboardItemsResponse>>> GetDashboardItemsAsync()
        {
            var dashboardResult = await _dashboardService.GetDashboardItemsAsync();
            if (dashboardResult == null)
            {
                return NoContent();
            }

            var result = dashboardResult.Select(ToDashboardItemsResponse);

            return Ok(result);
        }

        private static DashboardItemsResponse ToDashboardItemsResponse(SpGetDashboardItemsResult dashboardResult)
        {  
            return new DashboardItemsResponse { Name = dashboardResult.Name, TotalNumber = dashboardResult.TotalNumber };
        }
    } 
}

