using LetsDoIt.Moody.Domain;
using LetsDoIt.Moody.Infrastructure.ValueTypes;

namespace LetsDoIt.Moody.Application.User
{
    public class ToSystemUsersGetResult
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public Email Email { get; set; }

        public bool IsActive { get; set; }

        public UserType UserType { get; set; }
    }
}
