using LetsDoIt.Moody.Application.Data;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Search
{
    public class SearchService : ISearchService
    {
        IDataService _iDataService;
        public async Task<string> AutoCompleteSearch(string searchKey)
        {
            var value = await _iDataService.SearchFunction(searchKey);
            
            return value;
            
        }
    }
}
