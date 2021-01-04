using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsDoIt.Moody.Web.Controllers
{
    using Application.Constants;
    using Application.Dashboard;

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
    }
}
