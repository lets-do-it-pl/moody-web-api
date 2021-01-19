using System.Collections.Generic;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Search
{
    using LetsDoIt.Moody.Persistence.StoredProcedures.ResultEntities;
    public interface ISearchService
    {
        Task<ICollection<SpGetGeneralSearchResult>> GetDataResultAsync(string searchKey);
       
    }
}
