using System.Collections.Generic;

namespace LetsDoIt.Moody.Persistence.StoredProcedures.ResultEntities
{
    public class SpGetDashboardItemsResult
    {
        public string Name { get; set; }

        public int TotalNumber { get; set; }

        public ICollection<SpGetDashboardItemsResult> DashboardItems { get; set; }
    }
}
