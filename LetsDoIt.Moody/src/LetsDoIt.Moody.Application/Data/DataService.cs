using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFrameworkExtras.EFCore;

namespace LetsDoIt.Moody.Application.Data
{
    using Persistence;
    using Persistence.StoredProcedures;
    using Persistence.StoredProcedures.ResultEntities;
    

    public class DataService : IDataService
    {
        private readonly IApplicationContext _dbContext;
        
        public DataService(IApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection <SpGetGeneralSearchResult>> GetGeneralSearchResultAsync(string searchKey)
        {
            var input = new SpGeneralSearch { SearchValue = searchKey };

            var result = await _dbContext.Database.ExecuteStoredProcedureAsync<SpGetGeneralSearchResult>(input);

            if (result == null)
            {
                throw new NullReferenceException("NULL Search Result");
            }

            return result.ToArray();
        }

        public async Task<ICollection<SpGetDashboardItemsResult>> GetDashboardItemsAsync()
        {
            var input = new SpGetDashboardItems { };

            var result = await _dbContext.Database.ExecuteStoredProcedureAsync<SpGetDashboardItemsResult>(input);

            if (result == null)
            {
                return null;
            }

            return result.ToArray();
        }
    }
}
