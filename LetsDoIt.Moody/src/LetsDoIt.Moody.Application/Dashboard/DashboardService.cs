using LetsDoIt.Moody.Application.Data;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Dashboard
{
    using LetsDoIt.Moody.Persistence.StoredProcedures.ResultEntities;
    using Persistence.Repositories.Category;
    using System.Collections.Generic;

    public class DashboardService : IDashboardService
    {
        private readonly IDataService _dataService;
        private readonly ICategoryRepository _categoryRepository;
      
        public DashboardService( IDataService dataService)
        { 
            _dataService = dataService;
        }

        public async Task<ICollection<SpGetDashboardItemsResult>> GetDashboardItemsAsync()
        {
             var result = await _dataService.GetDashboardItemsAsync();
        
             return result.DashboardItems;  
        }
    }   
}
  