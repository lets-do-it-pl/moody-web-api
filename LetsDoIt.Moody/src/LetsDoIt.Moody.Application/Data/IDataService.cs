using System.Threading.Tasks;
namespace LetsDoIt.Moody.Application.Data
{
    using Persistence.StoredProcedures.ResultEntities;
    using System.Collections.Generic;

    public interface IDataService
    {
        Task<ICollection<SpGetDashboardItemsResult>> GetDashboardItemsAsync();
    }
}
