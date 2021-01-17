using LetsDoIt.MailSender.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LetsDoIt.Moody.Web.Extensions
{
    using Application.Options;

    public static class OptionsExtension
    {
        public static IServiceCollection AddOptionsConfig(
            this IServiceCollection services,
            IConfiguration configuration) => services
                .Configure<SmtpOptions>(configuration.GetSection(SmtpOptions.SmtpSectionName))
                .Configure<JwtOptions>(configuration.GetSection(JwtOptions.JwtSectionName))
                .Configure<WebInfoOptions>(configuration.GetSection(WebInfoOptions.WebInfoSectionName));
    }
}
