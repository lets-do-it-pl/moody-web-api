using System.ComponentModel.DataAnnotations;
using LetsDoIt.CustomValueTypes.Email;

namespace LetsDoIt.Moody.Web.Entities.Requests.User
{
    public class ForgetPasswordRequest
    {
        [Required]
        public Email Email { get; set; }
    }
}
