using System.Data;
using EntityFrameworkExtras.EFCore;

namespace LetsDoIt.Moody.Persistence.StoredProcedures
{
    [StoredProcedure("AutoCompleteSearch")]
    public class AutoCompleteSearch
    {
        [StoredProcedureParameter(SqlDbType.NVarChar, Direction = ParameterDirection.Input,ParameterName = "EnteredValue")]
        public string SearchValue {get; set; }


    }
}
