using LetsDoIt.Moody.Domain;
using LetsDoIt.Moody.Infrastructure.ValueTypes;

namespace LetsDoIt.Moody.Application.User
{
    public class UserEntity
    {
        public UserEntity(
            string username, 
            string encryptedPassword,
            bool isActive,
            UserType userType,
            string name,
            string surname,
            Email email)
        {
            Username = username;
            EncryptedPassword = encryptedPassword;
            UserType = userType;
            IsActive = isActive;
            Name = name;
            Surname = surname;
            Email = email;
        }

        public string Username { get; }

        public string EncryptedPassword { get; }

        public bool IsActive { get; }

        public UserType UserType { get; }

        public string Name { get; }

        public string Surname { get; }

        public Email Email { get; }
    }
}
