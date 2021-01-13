using System.Collections.Generic;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Search
{
    public interface ISearchService
    {
        Task<List<string>> DataSearchAsync(string searchKey);
       
    }
}
