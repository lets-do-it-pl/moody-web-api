namespace LetsDoIt.Moody.Application.Client
{
    public class ClientEntity
    {
        public ClientEntity(
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
