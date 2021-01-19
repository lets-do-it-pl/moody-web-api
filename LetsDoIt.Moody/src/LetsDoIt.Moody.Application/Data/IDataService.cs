using System.Threading.Tasks;
namespace LetsDoIt.Moody.Application.Data
{
    using Persistence.StoredProcedures.ResultEntities;
    public interface IDataService
    {
        Task<SpGetDashboardItemsResult[]> GetDashboardItemsAsync();
    }
}
