using System.Collections.Generic;
using System.Security.Claims;

namespace Globe.Tests.Mocks
{
    public class MockClaimsPrincipal
    {
        public static ClaimsPrincipal Mock()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "username"),
                new Claim(ClaimTypes.NameIdentifier, "userId"),
                new Claim("name", "John Doe"),
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            return claimsPrincipal;
        }
    }
}
