using Globe.Identity.Shared.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Globe.Identity.Resources.Extensions
{
    public static class GlobeAuthorizationExtensions
    {
        public static void AddGlobeAuthorization(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            IOptions<PolicyOptions> policyOptions = serviceProvider.GetService<IOptions<PolicyOptions>>();

            services.AddAuthorization(options =>
            {
                foreach (var policyOption in policyOptions.Value.Policies)
                {
                    options.AddPolicy(policyOption.Name, policy =>
                    {
                        foreach (var claim in policyOption.Claims)
                        {
                            policy.RequireClaim(claim.Type, claim.AllowValues);
                        }
                    });
                }
            });
        }
    }
}