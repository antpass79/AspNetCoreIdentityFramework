using Globe.Identity.Shared.Jwt;
using Globe.Identity.Shared.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Globe.Identity.Resources.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureGlobeOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<JwtAuthenticationOptions>(options => configuration.GetSection(nameof(JwtAuthenticationOptions)).Bind(options));
            services.Configure<PolicyOptions>(options => configuration.GetSection(nameof(PolicyOptions)).Bind(options));

            return services;
        }

        public static IServiceCollection InjectGlobeDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
            services.AddSingleton<ISigningCredentialsBuilder, SigningCredentialsBuilder>();

            return services;
        }

        public static IServiceCollection AddGlobeMiddlewares(this IServiceCollection services)
        {
            services.AddGlobeAuthorization();
            services.AddGlobeAuthentication();

            return services;
        }
    }
}
