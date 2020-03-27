using Globe.Identity.AdministrativeDashboard.Server.Models;
using Globe.Identity.Authentication.Jwt;
using Globe.Identity.Shared.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.Services
{
    public class UserRolesJwtTokenEncoder : JwtTokenEncoder<ApplicationUser>
    {
        public UserRolesJwtTokenEncoder(UserManager<ApplicationUser> userManager, IOptions<JwtAuthenticationOptions> options)
            : base(userManager, options)
        {
        }

        async protected override Task<IEnumerable<Claim>> BuildClaimsAsync(ApplicationUser input)
        {
            var roles = await UserManager.GetRolesAsync(input);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, input.UserName),
            }
            .Concat(roles
                .Select(role =>
                {
                    return new Claim(ClaimTypes.Role, role);
                }));

            return await Task.FromResult(claims);
        }
    }
}
