using System.Threading.Tasks;
using System.Collections.Generic;

namespace LetsDoIt.Moody.Application.Search
{
    using Data;
    using LetsDoIt.Moody.Persistence.StoredProcedures.ResultEntities;
    using System.Linq;

    public class SearchService : ISearchService
    {
        private readonly IDataService _dataService;
        public SearchService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<ICollection<SpGetGeneralSearchResult>> GetGeneralSearchResultAsync(string searchKey)
        {
            if (string.IsNullOrWhiteSpace(searchKey))
            {
                return Enumerable.Empty<SpGetGeneralSearchResult>().ToList();
            }

            return await _dataService.SpGetGeneralSearchResultAsync(searchKey.ToLower());
        }


    }
}
