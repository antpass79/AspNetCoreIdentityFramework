using Globe.Identity.Authentication.Jwt;
using Globe.Identity.Authentication.Options;
using Globe.Identity.Authentication.Services;
using Globe.Identity.Shared.Jwt;
using Globe.Identity.Shared.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Globe.Identity.Authentication.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureGlobeOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<DatabaseOptions>(options =>
            {
                configuration.GetSection(nameof(DatabaseOptions)).Bind(options);
                options.DefaultSqlServerConnection = configuration.GetConnectionString("DefaultSqlServerConnection");
                options.DefaultSqliteConnection = configuration.GetConnectionString("DefaultSqliteConnection");
            });
            services.Configure<JwtAuthenticationOptions>(options => configuration.GetSection(nameof(JwtAuthenticationOptions)).Bind(options));

            return services;
        }

        public static IServiceCollection InjectGlobeDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IJwtGenerator, JwtGenerator>();
            services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
            services.AddSingleton<ISigningCredentialsBuilder, SigningCredentialsBuilder>();

            services.AddSingleton<ILoginService, LoginService>();
            services.AddSingleton<IAccountsService, AccountsService>();

            return services;
        }

        public static IServiceCollection AddGlobeMiddlewares(this IServiceCollection services)
        {
            services.AddGlobeDbContext();
            services.AddGlobeAuthentication();
            services.AddGlobeIdentity();

            return services;
        }

    }
}
