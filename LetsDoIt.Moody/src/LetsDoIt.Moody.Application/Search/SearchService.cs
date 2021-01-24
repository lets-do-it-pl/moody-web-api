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
        private const string UserEntity = "User";
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

            if (Equals(results, Enumerable.Empty<SpGetGeneralSearchResult>()))
            {
                return Enumerable.Empty<SpGetGeneralSearchResult>().ToList();
            }

            if (userType != UserTypeConstants.Admin)
            {
                return results.Where(r => r.Name != UserEntity).ToArray();
            }

            return results;
        }
    }
}
