using System.Threading.Tasks;
using System.Collections.Generic;

namespace LetsDoIt.Moody.Application.Search
{
    using Constants;
    using Data;
    using Persistence.StoredProcedures.ResultEntities;
    using System.Linq;

    public class SearchService : ISearchService
    {
        private const string CategoryName = "Category";
        private readonly IDataService _dataService;
        public SearchService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<ICollection<SpGetGeneralSearchResult>> GetGeneralSearchResultAsync(string userType, string searchKey)
        {
            if (string.IsNullOrWhiteSpace(searchKey))
            {
                return Enumerable.Empty<SpGetGeneralSearchResult>().ToList();
            }

            var results = await _dataService.GetGeneralSearchResultAsync(searchKey.ToLower());

            if (userType == UserTypeConstants.Standard)
            {
                return results.Where(sggsr => sggsr.Name == CategoryName).ToArray();
            }

            return results;
        }
    }
}
