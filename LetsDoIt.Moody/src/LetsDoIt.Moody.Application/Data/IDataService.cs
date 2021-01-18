using LetsDoIt.Moody.Application.Search;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Data
{
    public interface IDataService
    {
        Task<ICollection<SearchGetResult>> GetSearchResultDbAsync(string searchKey);

    }
}
