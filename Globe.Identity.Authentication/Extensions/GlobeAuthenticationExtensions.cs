using Globe.Identity.Authentication.Options;
using Globe.Identity.Shared.Jwt;
using Globe.Identity.Shared.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Globe.Identity.Authentication.Extensions
{
    public static class GlobeAuthenticationExtensions
    {
        internal static void AddGlobeAuthentication(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            IOptions<JwtAuthenticationOptions> jwtAuthenticationOptions = serviceProvider.GetService<IOptions<JwtAuthenticationOptions>>();
            ISigningCredentialsBuilder signingCredentialsBuilder = serviceProvider.GetService<ISigningCredentialsBuilder>();
            signingCredentialsBuilder
                .AddSecretKey(jwtAuthenticationOptions.Value.SecretKey)
                .AddAlgorithm(SecurityAlgorithms.HmacSha256);

            services.Configure<JwtAuthenticationOptions>(options =>
            {
                options.Issuer = jwtAuthenticationOptions.Value.Issuer;
                options.Audience = jwtAuthenticationOptions.Value.Audience;
                options.SigningCredentials = signingCredentialsBuilder.Build();
            });

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer();
        }
    }
}
