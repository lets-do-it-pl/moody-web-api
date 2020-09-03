using System;
namespace LetsDoIt.Moody.Infrastructure
{
    public interface ITemporaryToken
    {
        public bool TemporaryTokenValidator(string guid);
    }
}
