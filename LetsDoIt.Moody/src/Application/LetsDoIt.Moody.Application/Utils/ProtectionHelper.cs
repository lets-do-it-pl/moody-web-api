using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace LetsDoIt.Moody.Application.Utils
{
    public static class ProtectionHelper
    {
        public static string EncryptValue(string value)
        {
            byte[] salt = new byte[128 / 8];

            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: value,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }
    }
}
