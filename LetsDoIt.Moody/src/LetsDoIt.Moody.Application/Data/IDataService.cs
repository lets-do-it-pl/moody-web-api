using System.Collections.Generic;
using System.Threading.Tasks;
using LetsDoIt.Moody.Persistence.StoredProcedures;
using LetsDoIt.Moody.Persistence.StoredProcedures.ResultEntities;

namespace LetsDoIt.Moody.Application.Data
{
    public interface IDataService
    {

        Task<ICollection<SbGeneralSearchResult>> GetGeneralSearchResultAsync(string search);
    }
}
