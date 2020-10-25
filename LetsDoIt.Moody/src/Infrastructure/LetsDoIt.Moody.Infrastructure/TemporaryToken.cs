using System;

namespace LetsDoIt.Moody.Infrastructure
{
    public class TemporaryToken
    {
        public static string GenerateTemporaryToken()
        {
            var saveUtcNow = DateTime.UtcNow;

            return $"Bearer " +
                   $"8e/{saveUtcNow.Year}" +
                   $"/53E/{saveUtcNow.Month}" +
                   $"/Sey/{saveUtcNow.Day}" +
                   "/KU25ecb/G5i4f46/74VHuMbg" +
                   "/f862021cf8f5==";
        }

        public static bool ValidateTemporaryToken(string tempToken) =>
            tempToken == GenerateTemporaryToken();
    }
}
