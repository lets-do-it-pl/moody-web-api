using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFrameworkExtras.EFCore;
using LetsDoIt.Moody.Persistence.StoredProcedures;

namespace LetsDoIt.Moody.Application.Data
{
    using Persistence;
    using Persistence.StoredProcedures.ResultEntities;

    public class DataService : IDataService
    {
        private readonly IApplicationContext _dbContext;

        public DataService(IApplicationContext dbContext)
        {
            _dbContext = dbContext;
           
        }


        public async Task<ICollection <SbGeneralSearchResult>> GetGeneralSearchResultAsync(string search)
        {
            var input = new SbGeneralSearch { SearchValue = search };

            var result = await _dbContext.Database.ExecuteStoredProcedureAsync<SbGeneralSearchResult>(input);

            if (result == null)
            {
                throw new NullReferenceException("NULL Search Result");
                
            }

            return result.ToList();
        }
    }
}
