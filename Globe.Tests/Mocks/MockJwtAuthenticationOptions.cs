using Globe.Identity.Options;
using Globe.Identity.Security;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Globe.Tests.Mocks
{
    public class MockJwtAuthenticationOptions
    {
        public IOptions<JwtAuthenticationOptions> Mock()
        {
            var signingCredentials = new SigningCredentialsBuilder()
                .AddSecretKey(IdentityContants.SECRET_KEY)
                .AddAlgorithm(SecurityAlgorithms.HmacSha256)
                .Build();

            var optionsWrapper = new OptionsWrapper<JwtAuthenticationOptions>(
                new JwtAuthenticationOptions
                {
                    Issuer = "webApi",
                    Audience = "http://localhost:5000/",
                    SigningCredentials = signingCredentials
                });

            return optionsWrapper;
        }
    }
}
