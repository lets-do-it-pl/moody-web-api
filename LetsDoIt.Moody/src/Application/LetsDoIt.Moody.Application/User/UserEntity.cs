using LetsDoIt.Moody.Domain;

namespace LetsDoIt.Moody.Application.User
{
    public class UserEntity
    {
        public UserEntity(
            string username, 
            string encryptedPassword, string name , string surname , string email ,
            UserTypes userType, bool isActive )
        {
            Username = username;
            EncryptedPassword = encryptedPassword;
            Name = name;
            Surname = surname;
            Email = email;
            UserType = userType;
            IsActive = isActive;
        }

        public string Username { get; }

        public string EncryptedPassword { get; }
        public string Name { get; }
        public string Surname { get; }
        public string Email { get; }
        public UserTypes UserType { get; }
        public bool IsActive { get; }
    }
}
