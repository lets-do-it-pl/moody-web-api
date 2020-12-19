using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Web.Cache
{
    public class RedisCacheSettings
    {
        public bool Enable { get; set; }

        public string ConnectionString { get; set; }
    }
}
