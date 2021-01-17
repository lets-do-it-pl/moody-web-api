using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.Constants;
    using Application.Dashboard;
    using System;
    using System.Collections.Generic;
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

       
        public async Task<ActionResult> GetDashboardItemsAsync(DateTime date)
        {
            /// response olmadan get methodunu nasil dondurucez ????
            var result = await _dashboardService.GetDashboardItemsAsync(date);
            if (result == null)
            {
                return NoContent();
            }

            return //??? response(result) ;
        }
       

    }
}
