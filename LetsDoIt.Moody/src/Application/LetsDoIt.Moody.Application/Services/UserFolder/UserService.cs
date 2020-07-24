using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using LetsDoIt.Moody.Domain.Entities;
using LetsDoIt.Moody.Persistance.DataAccess;

namespace LetsDoIt.Moody.Application.Services.UserFolder
{
    public class UserService:IUserService
    {
        private IUserRepository _userRepository;

        private const string _encryptionKey = "sakli";

        public UserService( IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void SaveUser(string userName, string password)
        {
            _userRepository.Add(new User
            {
                UserName = userName,
                Password = Encrypt(userName + password, _encryptionKey)

            });
        }


        //Example for soft delete by user Id. You first need to get(query) user information from database
        //because soft delete actually updates user information and if you send it empty, 
        //Then you would actually delete information by overriding it.

        //var user = _userRepository.Get(u => u.Id == 2);
        //_userRepository.Delete(user);

        public string Encrypt(string clearText, string EncryptionKey)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
    }
}
