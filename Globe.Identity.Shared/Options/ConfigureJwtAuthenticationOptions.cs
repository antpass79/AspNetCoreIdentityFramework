using Globe.Identity.Shared.Jwt;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Globe.Identity.Shared.Options
{
    public class ConfigureJwtAuthenticationOptions : IConfigureOptions<JwtAuthenticationOptions>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public ConfigureJwtAuthenticationOptions(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Configure(JwtAuthenticationOptions options)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var signingCredentialsBuilder = scope.ServiceProvider.GetRequiredService<ISigningCredentialsBuilder>();
                options.SigningCredentials = signingCredentialsBuilder
                    .AddSecretKey(options.SecretKey)
                    .AddAlgorithm(SecurityAlgorithms.HmacSha256)
                    .Build();
            }
        }
    }
}
