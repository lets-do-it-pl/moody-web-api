using System.Threading.Tasks;
using System.Collections.Generic;

namespace LetsDoIt.Moody.Application.Search
{
    using Data;
    using LetsDoIt.Moody.Persistence.StoredProcedures.ResultEntities;

    public class SearchService : ISearchService
    {
        private readonly IDataService _dataService;
        public SearchService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<ICollection<SpGetGeneralSearchResult>> GetDataResultAsync(string searchKey)
        {
            searchKey = searchKey.ToLower();
            return await _dataService.GetGeneralSearchResultAsync(searchKey);
        }

       
    }
}
