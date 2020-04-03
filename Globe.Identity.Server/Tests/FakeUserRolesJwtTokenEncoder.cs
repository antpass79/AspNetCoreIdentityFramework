using Globe.Identity.Models;
using Globe.Identity.Options;
using Globe.Identity.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Globe.Identity.Server.Tests
{
    public class FakeUserRolesJwtTokenEncoder : UserRolesJwtTokenEncoder<GlobeUser>
    {
        public FakeUserRolesJwtTokenEncoder(UserManager<GlobeUser> userManager, IOptions<JwtAuthenticationOptions> options)
            : base(userManager, options)
        {
        }

        async protected override Task<IEnumerable<Claim>> BuildClaimsAsync(GlobeUser input)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, input.UserName),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "UserManager"),
                new Claim(ClaimTypes.Role, "Guest"),
            };

            return await Task.FromResult(claims);
        }
    }
}
