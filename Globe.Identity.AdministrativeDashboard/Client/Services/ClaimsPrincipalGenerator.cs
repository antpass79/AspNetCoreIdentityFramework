using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Globe.Identity.AdministrativeDashboard.Client.Services
{
    public static class ClaimsPrincipalGenerator
    {
        public static ClaimsPrincipal BuildClaimsPrincipal(string savedToken, string userName)
        {
            var roleClaims = ParseClaimsFromJwt(savedToken);
            var userNameClaims = new[] { new Claim(ClaimTypes.Name, userName) };
            var claims = roleClaims.ToList().Concat(userNameClaims);

            return new ClaimsPrincipal(new ClaimsIdentity(claims, "principal"));
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            Console.WriteLine("BEFORE");
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            Console.WriteLine("AFTER");
            Console.WriteLine(token.Claims.Where(claim => claim.Type == ClaimTypes.Role).Count());
            return token.Claims.Where(claim => claim.Type == ClaimTypes.Role);
        }

    }
}
