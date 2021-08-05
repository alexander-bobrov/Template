using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Database.Configuration
{
    public static class DatabaseExtension
    {
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetSection(nameof(DatabaseOptions)).Get<DatabaseOptions>();
            
            services.AddDbContext<TemplateContext>(x => x.UseSqlite(options.ConnectionString), optionsLifetime: ServiceLifetime.Singleton);
            services.AddDbContextFactory<TemplateContext>(x => x.UseSqlite(options.ConnectionString));
        }
    }
}