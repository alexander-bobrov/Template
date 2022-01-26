using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Services.MessageService;

namespace Template.Services.CleanupService.Configuration
{
    public static class MessageExtension
    {
        public static void UseMessageService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMessageService, MessageService.MessageService>();
        }
    }
}
