using System.Threading.Tasks;
using System.Linq;
using EntityFrameworkExtras.EFCore;

namespace LetsDoIt.Moody.Application.Data
{
    using Persistence;
    using Persistence.StoredProcedures;
    using Persistence.StoredProcedures.ResultEntities;
    using System.Collections.Generic;

    public class DataService : IDataService
    {
        private readonly IApplicationContext _dbContext;

        public DataService(IApplicationContext dbContext)
        {
            _dbContext = dbContext;
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
