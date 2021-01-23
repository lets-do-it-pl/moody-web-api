using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.Constants;
    using Application.Dashboard;
    using Entities.Responses;
    using Persistence.StoredProcedures.ResultEntities;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DashboardItemsResponse>>> GetDashboardItemsAsync()
        {
            var dashboardResult = await _dashboardService.GetDashboardItemsAsync();
            if (dashboardResult == null)
            {
                return NoContent();
            }

            return Ok(dashboardResult.Select(ToDashboardItemsResponse));
        }

        private static DashboardItemsResponse ToDashboardItemsResponse(SpGetDashboardItemsResult dashboardResult) => new DashboardItemsResponse
        {
            Name = dashboardResult.Name,
            TotalNumber = dashboardResult.TotalNumber
        };
    } 
}

