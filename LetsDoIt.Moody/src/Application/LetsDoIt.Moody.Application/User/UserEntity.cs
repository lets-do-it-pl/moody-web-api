using LetsDoIt.Moody.Domain;

namespace LetsDoIt.Moody.Application.User
{
    public class UserEntity
    {
        public UserEntity(
            string username, 
            string encryptedPassword, bool isActive, UserTypes userType, string name , string surname , string email)
        {
            Username = username;
            EncryptedPassword = encryptedPassword;
            IsActive = isActive;
            UserType = userType;
            Name = name;
            Surname = surname;
            Email = email;
        }

        public string Username { get; }

        public string EncryptedPassword { get; }

        public bool IsActive { get; }

        public UserTypes UserType { get; }

        public string Name { get; }

        public string Surname { get; }

        public string Email { get; }
    }
}
