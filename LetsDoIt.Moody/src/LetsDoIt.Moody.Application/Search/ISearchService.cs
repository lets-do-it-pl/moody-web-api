using System.Collections.Generic;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Search
{
    using Persistence.StoredProcedures.ResultEntities;

    public interface ISearchService
    {
        Task<ICollection<SpGetGeneralSearchResult>> GetGeneralSearchResultAsync(string userType, string searchKey);

    }
}
