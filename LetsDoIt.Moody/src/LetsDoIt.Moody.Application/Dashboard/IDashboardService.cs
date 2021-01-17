using LetsDoIt.Moody.Persistence.StoredProcedures.ResultEntities;
using System;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Application.Dashboard
{
    public interface IDashboardService 
    {
        Task<SpGetDashboardItemsResult> GetDashboardItemsAsync(DateTime date);
    }
}
