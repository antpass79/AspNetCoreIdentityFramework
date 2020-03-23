using Globe.Identity.Authentication.Jwt;
using Globe.Identity.Shared.Jwt;
using Globe.Identity.Shared.Options;
using Globe.Tests;
using Globe.Tests.Mocks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using Xunit;

namespace Globe.AuthenticationServer.Tests
{
    public class JwtFactoryTests
    {
        [Fact]
        public void InvalidOptions()
        {
            IOptions<JwtAuthenticationOptions> options = Microsoft.Extensions.Options.Options.Create<JwtAuthenticationOptions>(null);
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new JwtGenerator(options));

            Assert.Equal(nameof(options), exception.ParamName);
        }

        [Fact]
        public void InvalidSigningCredentials()
        {
            IOptions<JwtAuthenticationOptions> options = Microsoft.Extensions.Options.Options.Create(new JwtAuthenticationOptions());
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new JwtGenerator(options));

            Assert.Equal(nameof(JwtAuthenticationOptions.SigningCredentials), exception.ParamName);
        }

        [Fact]
        public void InvalidValidForLessThanZero()
        {
            IOptions<JwtAuthenticationOptions> options = Microsoft.Extensions.Options.Options.Create(new JwtAuthenticationOptions
            {
                ValidFor = TimeSpan.MinValue
            });
            ArgumentException exception = Assert.Throws<ArgumentException>(() => new JwtGenerator(options));

            Assert.Equal(nameof(JwtAuthenticationOptions.ValidFor), exception.ParamName);
        }

        [Fact]
        public void InvalidValidForEqualToZero()
        {
            IOptions<JwtAuthenticationOptions> options = Microsoft.Extensions.Options.Options.Create(new JwtAuthenticationOptions
            {
                ValidFor = TimeSpan.Zero
            });
            ArgumentException exception = Assert.Throws<ArgumentException>(() => new JwtGenerator(options));

            Assert.Equal(nameof(JwtAuthenticationOptions.ValidFor), exception.ParamName);
        }

        [Fact]
        async public void GenerateEncodedTokenWithValidData()
        {
            var fakeIdentity = MockClaimsIdentity.Mock();
            ISigningCredentialsBuilder signingCredentialsBuilder = new SigningCredentialsBuilder();
            var fakeSigningCredentials = signingCredentialsBuilder
                .AddSecretKey(IdentityContants.SECRET_KEY)
                .AddAlgorithm(SecurityAlgorithms.HmacSha256)
                .Build();

            IOptions<JwtAuthenticationOptions> options = Microsoft.Extensions.Options.Options.Create(new JwtAuthenticationOptions
            {
                Audience = "http://localhost:5000/",
                Issuer = "webApi",
                SigningCredentials = fakeSigningCredentials
            });
            var jwtFactory = new JwtGenerator(options);
            var jwt = await jwtFactory.GenerateEncodedToken("administratorUser", fakeIdentity);

            Assert.NotEqual(String.Empty, jwt);
        }

        [Fact]
        public void GenerateClaimsIdentityWithValidData()
        {
            var fakeIdentity = MockClaimsIdentity.Mock();
            ISigningCredentialsBuilder signingCredentialsBuilder = new SigningCredentialsBuilder();
            var fakeSigningCredentials = signingCredentialsBuilder
                .AddSecretKey(IdentityContants.SECRET_KEY)
                .AddAlgorithm(SecurityAlgorithms.HmacSha256)
                .Build();

            IOptions<JwtAuthenticationOptions> options = Microsoft.Extensions.Options.Options.Create(new JwtAuthenticationOptions
            {
                Audience = "http://localhost:5000/",
                Issuer = "webApi",
                SigningCredentials = fakeSigningCredentials
            });
            var jwtFactory = new JwtGenerator(options);
            var idText = Guid.NewGuid().ToString();
            var claimsIdentity = jwtFactory.GenerateClaimsIdentity("AdministratorUser", idText);

            Assert.Equal("AdministratorUser", claimsIdentity.Name);
            Assert.Equal("Token", claimsIdentity.AuthenticationType);
            Assert.Equal("id", claimsIdentity.Claims.ElementAt(1).Type);
            Assert.Equal(idText, claimsIdentity.Claims.ElementAt(1).Value);
            Assert.Equal("administrator", claimsIdentity.Claims.ElementAt(2).Type);
            Assert.Equal("full_access", claimsIdentity.Claims.ElementAt(2).Value);
        }
    }
}
