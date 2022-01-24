using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Template.Services.AccountService;

namespace Template.Configuration
{
    public static class AuthExtension
    {
        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetSection(nameof(SecurityOptions));
            services.Configure<SecurityOptions>(options);

            var key = options.Get<SecurityOptions>().SecretKey;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    };
                });

            services.AddScoped<AccountService>();
        }
    }
}