using System.ComponentModel.DataAnnotations;

namespace LetsDoIt.Moody.Web.Entities.Requests
{
    public class SaveClientRequest
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
