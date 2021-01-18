using LetsDoIt.Moody.Persistence.StoredProcedures.ResultEntities;
using System.Collections.Generic;

namespace LetsDoIt.Moody.Web.Entities.Responses
{
    public class DashboardItemsResponse
    {
        public string Name { get; set; }

        public int TotalNumber { get; set; }

        public ICollection<SpGetDashboardItemsResult> DashboardItems { get; set; }
    }
}
