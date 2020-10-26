using System.ComponentModel.DataAnnotations;
using LetsDoIt.Moody.Domain;

namespace LetsDoIt.Moody.Web.Entities.Requests
{
    public class SaveUserRequest
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }
    }
}
