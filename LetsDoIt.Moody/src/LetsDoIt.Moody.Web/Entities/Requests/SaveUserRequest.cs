using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace LetsDoIt.Moody.Web.Entities.Requests
{
    public class SaveUserRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public string UserType { get; set; }

        [Required]
        public int CreatedBy { get; set; }
    }
}
