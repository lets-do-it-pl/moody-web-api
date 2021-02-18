using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Web.Entities.Responses
{
    public class AccountResponse
    {
        public byte[] Image { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }
    }
}
