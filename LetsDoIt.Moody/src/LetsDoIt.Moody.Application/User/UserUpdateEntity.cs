using LetsDoIt.CustomValueTypes.Email;

namespace LetsDoIt.Moody.Application.User
{
    public class UserUpdateEntity
    {
        public string Password { get; set; }

        public bool CanLogin { get; set; }

        public bool IsActive { get; set; }

        public string UserType { get; set; }

        public string FullName { get; set; }

        public Email Email { get; set; }

        public int Id { get; set; }

        public int ModifiedById { get; set; }
    }
}
