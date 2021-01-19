using System.Collections.Generic;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Data
{
    using LetsDoIt.Moody.Persistence.StoredProcedures.ResultEntities;
    public interface IDataService
    {
        Task<ICollection<SpGetGeneralSearchResult>> GetGeneralSearchResultAsync(string search);
    }
}
