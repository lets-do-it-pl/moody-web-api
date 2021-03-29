using System.ComponentModel.DataAnnotations;
using LetsDoIt.CustomValueTypes.Email;

namespace LetsDoIt.Moody.Web.Entities.Requests.User
{
    public class SaveUserRequest
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public Email Email { get; set; }
    }
}
