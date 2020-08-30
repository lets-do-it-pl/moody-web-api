using System;

namespace LetsDoIt.Moody.TemporaryTokenGenerator
{
    using Infrastructure;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(TemporaryToken.TemporaryTokenGenerator());
        }
    }
}
