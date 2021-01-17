using LetsDoIt.Moody.Application.Data;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Dashboard
{
    using LetsDoIt.Moody.Persistence.StoredProcedures;
    using LetsDoIt.Moody.Persistence.StoredProcedures.ResultEntities;
    using Persistence.Entities;
    using Persistence.Repositories.Base;
    using Persistence.Repositories.Category;
    using System;

    public class DashboardService : IDashboardService
    {
        private readonly IDashboardService _dashboardRepository;
        private readonly IDataService _dataService;
        private readonly ICategoryRepository _categoryRepository;
        public DashboardService(IDashboardService dashboardRepository, IDataService dataService)
        {
            _dashboardRepository = dashboardRepository;
            _dataService = dataService;


        }

        public async Task<SpGetDashboardItemsResult> GetDashboardItemsAsync(DateTime date)
        {

            var result = await _dataService.GetDashboardItemsAsync(date);
            if (result == null)
            {
                return new SpGetDashboardItemsResult();
            }

            return result.FirstOrDefault();
        }
    }
    
}
  