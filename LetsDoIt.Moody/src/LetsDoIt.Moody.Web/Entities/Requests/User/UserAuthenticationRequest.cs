using System.ComponentModel.DataAnnotations;
using LetsDoIt.CustomValueTypes.Email;

namespace LetsDoIt.Moody.Web.Entities.Requests.User
{
    public class UserAuthenticationRequest
    {
        [Required]
        public Email Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
