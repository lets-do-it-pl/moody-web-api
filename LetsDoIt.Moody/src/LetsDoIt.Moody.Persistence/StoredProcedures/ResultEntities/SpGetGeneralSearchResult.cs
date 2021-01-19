using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using EntityFrameworkExtras.EFCore;

namespace LetsDoIt.Moody.Persistence.StoredProcedures.ResultEntities
{
    public class SpGetGeneralSearchResult
    {
        public string Name {get;set;}
        public string Type {get;set;}
    }
}
