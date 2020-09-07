using System;
using McMaster.Extensions.CommandLineUtils;

namespace LetsDoIt.Moody.TemporaryTokenGenerator
{
    using Infrastructure;

    class Program
    {
        public static void Main(string[] args)
        {
            var app = new CommandLineApplication
            {
                Name = "TemporaryToken",
                Description = "Generate temporary token for saving user operation"
            };
            app.HelpOption("-?|-h|--help");


            app.OnExecute(() =>
            {
                var tokenValue = TemporaryToken.GenerateTemporaryToken();

                Console.WriteLine($"Generated token: {tokenValue}");
                
                return 0;
            });

            app.Execute(args);
        }
    }
}
