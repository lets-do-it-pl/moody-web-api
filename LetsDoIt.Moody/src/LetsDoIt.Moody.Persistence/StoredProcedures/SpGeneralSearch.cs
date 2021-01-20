using System.Data;
using EntityFrameworkExtras.EFCore;

namespace LetsDoIt.Moody.Persistence.StoredProcedures
{
    [StoredProcedure("SpGeneralSearch")]
    public class SpGeneralSearch
    {
        [StoredProcedureParameter(SqlDbType.VarChar, Direction = ParameterDirection.Input, ParameterName = "searchKey")]
        public string SearchValue {get;set;}
    }
}
