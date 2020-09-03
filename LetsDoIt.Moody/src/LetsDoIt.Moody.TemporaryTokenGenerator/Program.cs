using System;
using Microsoft.Extensions.CommandLineUtils;

namespace LetsDoIt.Moody.TemporaryTokenGenerator
{ 
    using Infrastructure;

    class Program
    {
        public static void Main(params string[] args)
        {
            var application = new CommandLineApplication();
            application.Name = "TemporaryToken";
            application.Description = "";
            application.HelpOption("-?|-h|--help");

            application.OnExecute(() =>
            {
                Console.WriteLine($"Generated token is {TemporaryToken.TemporaryTokenGenerator()}");
                return 0;
            });
        }
    }
}
