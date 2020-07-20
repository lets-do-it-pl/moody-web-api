using System;
using System.Collections.Generic;
using System.Text;

namespace LetsDoIt.Moody.Domain
{
    public class User
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime CreateDate { get; } //Date will be created automatically
        public bool isDeleted { get; set; } = false;
    }
}
