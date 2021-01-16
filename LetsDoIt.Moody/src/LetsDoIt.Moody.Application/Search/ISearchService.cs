using System.Collections;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Search
{
    public interface ISearchService
    {
        Task<IEnumerable> GeneralSearchAsync(string searchKey);
       
    }
}
