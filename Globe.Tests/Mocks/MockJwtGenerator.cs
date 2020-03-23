using Globe.Identity.Authentication.Jwt;

namespace Globe.Tests.Mocks
{
    public class MockJwtGenerator
    {
        public IJwtGenerator Mock()
        {
            return new JwtGenerator(new MockJwtAuthenticationOptions().Mock());
        }
    }
}
