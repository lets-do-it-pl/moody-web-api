﻿using LetsDoIt.Moody.Application.Data;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Dashboard
{
    using LetsDoIt.Moody.Persistence.StoredProcedures.ResultEntities;
    using System.Collections.Generic;

    public class DashboardService : IDashboardService
    {
        private readonly IDataService _dataService;
      
        public DashboardService( IDataService dataService)
        { 
            _dataService = dataService;
        }

        public async Task<ICollection<SpGetDashboardItemsResult>> GetDashboardItemsAsync()
        {
            return await _dataService.GetDashboardItemsAsync();
        }
    }   
}
  