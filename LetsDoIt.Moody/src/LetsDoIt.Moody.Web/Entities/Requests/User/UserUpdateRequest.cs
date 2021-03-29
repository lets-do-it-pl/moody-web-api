using System.ComponentModel.DataAnnotations;
using LetsDoIt.CustomValueTypes.Email;

namespace LetsDoIt.Moody.Web.Entities.Requests.User
{
    public class UserUpdateRequest
    {
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public Email Email { get; set; }

        [Required]
        public string UserType { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public bool CanLogin { get; set; }
    }
}
