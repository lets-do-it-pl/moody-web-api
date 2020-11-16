using System;
using System.Collections.Generic;

namespace LetsDoIt.Moody.Persistence.Entities
{
    public partial class VersionHistory
    {
        public long Id { get; set; }
        public string VersionNumber { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
