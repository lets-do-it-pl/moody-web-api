using LetsDoIt.Moody.Application.Data;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Search
{
    public class SearchService : ISearchService
    {
        IDataService _iDataService;

        public async Task<IEnumerable> GeneralSearchAsync(string searchKey)
        {
           
            var value = await _iDataService.SearchFunctionDatabese(searchKey);

            return value;
        }
    }
    
        

    }
