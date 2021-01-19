using System.Data;
using EntityFrameworkExtras.EFCore;

namespace LetsDoIt.Moody.Persistence.StoredProcedures
{
    [StoredProcedure("GeneralSearch")]
    public class SbGeneralSearch
    {
        [StoredProcedureParameter(SqlDbType.NVarChar, Direction = ParameterDirection.Input, ParameterName = "EnteredValue")]
        public string SearchValue { get; set; }
    }
}
