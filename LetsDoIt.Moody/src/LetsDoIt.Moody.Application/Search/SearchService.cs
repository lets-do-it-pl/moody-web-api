using LetsDoIt.Moody.Application.Data;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Search
{
    public class SearchService : ISearchService
    {
        IDataService _dataService;
        public SearchService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<IEnumerable> GeneralSearchAsync(string searchKey)
        {

            var value = await _dataService.SearchFunctionDatabese(searchKey);

            return value;
        }
    }



}
