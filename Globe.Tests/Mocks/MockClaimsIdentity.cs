using System.Collections.Generic;
using System.Security.Claims;

namespace Globe.Tests.Mocks
{
    public static class MockClaimsIdentity
    {
        public static ClaimsIdentity Mock()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "administratorUser")
            };

            var identity = new ClaimsIdentity(claims, "IdentityTest");
            return identity;
        }
    }
}
