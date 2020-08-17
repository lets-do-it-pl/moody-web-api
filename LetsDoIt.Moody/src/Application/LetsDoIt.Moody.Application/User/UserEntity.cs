namespace LetsDoIt.Moody.Application.User
{
    public class UserEntity
    {
        public UserEntity(
            string username, 
            string encryptedPassword)
        {
            Username = username;
            EncryptedPassword = encryptedPassword;
        }

        public string Username { get; }

        public string EncryptedPassword { get; }
    }
}
