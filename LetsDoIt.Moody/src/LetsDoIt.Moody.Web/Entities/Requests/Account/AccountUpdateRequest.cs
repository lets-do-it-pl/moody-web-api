using System.ComponentModel.DataAnnotations;
using LetsDoIt.CustomValueTypes.Email;
using LetsDoIt.CustomValueTypes.Image;

namespace LetsDoIt.Moody.Web.Entities.Requests.Account
{
    public class AccountUpdateRequest
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public Email Email { get; set; }

        public Image Image { get; set; }
    }
}
