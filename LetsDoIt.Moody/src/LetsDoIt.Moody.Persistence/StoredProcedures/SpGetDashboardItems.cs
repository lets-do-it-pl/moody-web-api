using EntityFrameworkExtras.EFCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LetsDoIt.Moody.Persistence.StoredProcedures
{
    [StoredProcedure("SpGetDashboardItems")]
    public class SpGetDashboardItems
    {
        [StoredProcedureParameter(SqlDbType.DateTime, Direction = ParameterDirection.Input, ParameterName = "date")]
        public DateTime Date { get; set; }
    }
}
