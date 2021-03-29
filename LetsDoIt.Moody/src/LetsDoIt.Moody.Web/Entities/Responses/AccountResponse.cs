using LetsDoIt.CustomValueTypes.Email;

namespace LetsDoIt.Moody.Web.Entities.Responses
{
    public class AccountResponse
    {
        public string Image { get; set; }

        public string FullName { get; set; }

        public Email Email { get; set; }
    }
}
