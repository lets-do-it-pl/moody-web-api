using System.ComponentModel.DataAnnotations;

namespace LetsDoIt.Moody.Web.Entities.Requests.User
{
    public class SaveUserRequest
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
