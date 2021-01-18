using LetsDoIt.Moody.Persistence.StoredProcedures.ResultEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Dashboard
{
    public interface IDashboardService 
    {
        Task<ICollection<SpGetDashboardItemsResult>> GetDashboardItemsAsync();
    }
}
