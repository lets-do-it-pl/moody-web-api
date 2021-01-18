using System.Collections.Generic;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Search
{
    public interface ISearchService
    {
        Task<ICollection<SearchGetResult>> GetGeneralSearchResultAsync(string searchKey);
       
    }
}
