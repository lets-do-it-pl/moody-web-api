using System.ComponentModel.DataAnnotations;

namespace LetsDoIt.Moody.Web.Entities.Requests
{
    public class ForgetPasswordRequest
    {
        [Required]
        public string Email { get; set; }
    }
}
