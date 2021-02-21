using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Web.Entities.Requests.Account
{
    public class AccountUpdateRequest
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }


        public string Image { get; set; }

    }
}
