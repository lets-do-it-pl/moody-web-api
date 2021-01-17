using System;
using System.Threading.Tasks;
using System.Linq;
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


        public async Task<SpGetDashboardItemsResult> GetDashboardItemsAsync(DateTime date)
        {
            var input = new SpGetDashboardItems { Date = date };

            var result = await _dbContext.Database.ExecuteStoredProcedureAsync<SpGetDashboardItemsResult>(input);

            if (result == null)
            {
                return new SpGetDashboardItemsResult();
            }

            return result.FirstOrDefault();
        }
    }
}
