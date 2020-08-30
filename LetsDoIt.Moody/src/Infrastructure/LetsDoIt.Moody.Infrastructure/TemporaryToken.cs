using System;

namespace LetsDoIt.Moody.Infrastructure
{
    public class TemporaryToken
    { 
        public static string TemporaryTokenGenerator()
        {
            DateTime saveUtcNow = DateTime.UtcNow;
            return
                $"8e/{saveUtcNow.Year}/53E/{saveUtcNow.Month}/Sey/" +
                $"{saveUtcNow.Day}/KU2/{saveUtcNow.Hour}/G5i/{saveUtcNow.Minute}/74VHuMbg==";
        }

        public string TemporaryTokenValidator(string guid)
        {
            if(guid != TemporaryTokenGenerator() || guid == null)
            {
                throw new UnauthorizedAccessException();
            }

            return guid;
        }
    }
}
