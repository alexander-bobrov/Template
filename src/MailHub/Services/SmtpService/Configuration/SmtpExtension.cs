using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MailHub.Services.MailService.Configuration
{
    public static class SmtpExtension
    {
        public static void UseSmtpService(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetSection(nameof(SmtpOptions));
            services.Configure<SmtpOptions>(options);
            services.AddHostedService<SmtpService>();
        }
    }
}
