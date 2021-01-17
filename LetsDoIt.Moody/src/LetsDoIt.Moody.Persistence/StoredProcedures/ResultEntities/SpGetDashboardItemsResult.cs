using System;
using System.Collections.Generic;
using System.Text;

namespace LetsDoIt.Moody.Persistence.StoredProcedures.ResultEntities
{
   public class SpGetDashboardItemsResult
    {
        public string Name { get; set; }

        public DateTime CurrentDate { get; set; }

        public SpGetDashboardItemsResult FirstOrDefault()
        {
            throw new NotImplementedException();
        }
    }
}
