using System.Collections;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Search
{
    using Data;

    public class SearchService : ISearchService
    {
        IDataService _dataService;
        public SearchService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<IEnumerable> GeneralSearchAsync(string searchKey)
        {

            var value = await _dataService.SearchFunctionDatabase(searchKey.ToLower());

            return value;
        }
    }



}
