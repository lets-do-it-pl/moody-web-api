using System;
using System.Collections.Generic;

namespace LetsDoIt.Moody.Persistence.Entities
{
    public partial class Client
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
