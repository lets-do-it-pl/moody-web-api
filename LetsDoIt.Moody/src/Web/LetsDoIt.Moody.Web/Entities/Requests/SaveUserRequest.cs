using System.ComponentModel.DataAnnotations;
using LetsDoIt.Moody.Domain;

namespace LetsDoIt.Moody.Web.Entities.Requests
{
    public class SaveUserRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public UserTypes UserType { get; set; }

    }
}
