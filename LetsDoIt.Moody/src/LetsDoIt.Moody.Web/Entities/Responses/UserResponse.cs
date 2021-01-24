using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsDoIt.Moody.Web.Entities.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool CanLogin { get; set; }
        public string UserType { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
