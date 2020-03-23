using Globe.Identity.Authentication.Data;
using Globe.Identity.Authentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Globe.Identity.Authentication.Extensions
{
    public static class GlobeIdentityExtensions
    {
        public static void AddGlobeIdentity(this IServiceCollection services)
        {
            services.AddIdentity<GlobeUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 3;
            })
                .AddEntityFrameworkStores<GlobeDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}
