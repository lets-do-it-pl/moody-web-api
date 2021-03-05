using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace LetsDoIt.Moody.Application.Data
{
    using Persistence.StoredProcedures.ResultEntities;

    public interface IDataService
    {
        Task<ICollection<SpGetGeneralSearchResult>> GetGeneralSearchResultAsync(string searchKey);

        Task<ICollection<SpGetDashboardItemsResult>> GetDashboardItemsAsync();

        IEnumerable<CategoryUserReturnResult> GetUsers();
    }
}