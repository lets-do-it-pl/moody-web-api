using System;
using System.Linq;
using System.Threading.Tasks;
using EntityFrameworkExtras.EFCore;

namespace LetsDoIt.Moody.Application.Data
{
    using Persistence;
    using Persistence.StoredProcedures;

    public class DataService : IDataService
    {
        private readonly IApplicationContext _dbContext;

        public DataService(IApplicationContext dbContext)
        {
            _dbContext = dbContext;
           
        }


        public async Task<AutoCompleteSearch> GetAutoCompleteSearchAsync(string search)
        {
            var input = new AutoCompleteSearch { SearchValue = search };

            var result = await _dbContext.Database.ExecuteStoredProcedureAsync<AutoCompleteSearch>(input);

            if (result == null)
            {
                return new AutoCompleteSearch();
            }

            return result.FirstOrDefault();
        }
    }
}
