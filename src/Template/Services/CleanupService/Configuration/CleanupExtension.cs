using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Template.Services.CleanupService.Configuration
{
    public static class CleanupExtension
    {
        public static void UseBackgroundCleanup(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetSection(nameof(CleanupOptions));
            services.Configure<CleanupOptions>(options);

            services.AddHostedService<CleanupService>();
        }
    }
}
