using System.ComponentModel.DataAnnotations;

namespace LetsDoIt.Moody.Web.Entities.Requests.User
{
    public class ResetPasswordRequest
    {
        [Required]
        public string Password { get; set; }
    }
}
