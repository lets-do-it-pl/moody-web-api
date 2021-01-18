using System.Threading.Tasks;
using System.Collections.Generic;

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

        public async Task<ICollection<SearchGetResult>> GetGeneralSearchResultAsync(string searchKey)
        {
            searchKey = TurnToLowerCase(searchKey);
            return await _dataService.GetSearchResultDbAsync(searchKey);
        }

        public string TurnToLowerCase(string searchKey)
        {   
            if(searchKey== null)
            {
                throw new System.Exception("Null SearchKey");
            }
             return searchKey.ToLower();
        }
    }
}
