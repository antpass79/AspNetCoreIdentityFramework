using Globe.Identity.Models;
using Globe.Identity.Security;
using Globe.Identity.Services;
using Globe.Tests.Mocks;
using Xunit;

namespace Globe.Identity.Tests
{
    public class LoginServiceTests
    {
        [Fact]
        async public void LoginWithNotExistsCredentials()
        {
            var userManager = FakeUserManager.Mock();
            var signInManager = FakeSignInManager.Mock(userManager);
            var jwtIssuerOptions = new MockJwtAuthenticationOptions().Mock();
            var jwtTokenEncoder = new JwtTokenEncoder<GlobeUser>(userManager, jwtIssuerOptions);

            var loginService = new LoginService(userManager, signInManager, jwtIssuerOptions, jwtTokenEncoder);
            var loginResult = await loginService.LoginAsync(new Credentials
            {
                UserName = "user3",
                Password = "user3@email.com"
            });

            Assert.False(loginResult.Successful);
            Assert.Equal("Invalid credentials", loginResult.Error);
        }

        [Fact]
        async public void LoginWithInvalidCredentials()
        {
            var userManager = FakeUserManager.Mock();
            FakeUserManager.FillWithMockUsers(userManager);
            var signInManager = FakeSignInManager.Mock(userManager);
            var jwtIssuerOptions = new MockJwtAuthenticationOptions().Mock();
            var jwtTokenEncoder = new JwtTokenEncoder<GlobeUser>(userManager, jwtIssuerOptions);

            var loginService = new LoginService(userManager, signInManager, jwtIssuerOptions, jwtTokenEncoder);
            var loginResult = await loginService.LoginAsync(new Credentials
            {
                UserName = "user2",
                Password = "user2@passwordNotValid"
            });

            Assert.False(loginResult.Successful);
            Assert.Equal("Invalid credentials", loginResult.Error);
        }

        [Fact]
        async public void LoginWithValidCredentials()
        {
            var userManager = FakeUserManager.Mock();
            FakeUserManager.FillWithMockUsers(userManager);
            var signInManager = FakeSignInManager.Mock(userManager);
            var jwtIssuerOptions = new MockJwtAuthenticationOptions().Mock();
            var jwtTokenEncoder = new JwtTokenEncoder<GlobeUser>(userManager, jwtIssuerOptions);

            var loginService = new LoginService(userManager, signInManager, jwtIssuerOptions, jwtTokenEncoder);
            var loginResult = await loginService.LoginAsync(new Credentials
            {
                UserName = "user2",
                Password = "user2@password"
            });

            Assert.True(loginResult.Successful);
            Assert.NotNull(loginResult.Token);
            Assert.NotEmpty(loginResult.Token);
        }

        [Fact]
        async public void LogoutWithNotExistsUser()
        {
            var userManager = FakeUserManager.Mock();
            var signInManager = FakeSignInManager.Mock(userManager);
            var jwtIssuerOptions = new MockJwtAuthenticationOptions().Mock();
            var jwtTokenEncoder = new JwtTokenEncoder<GlobeUser>(userManager, jwtIssuerOptions);

            var loginService = new LoginService(userManager, signInManager, jwtIssuerOptions, jwtTokenEncoder);
            await loginService.LogoutAsync(new Credentials
            {
                UserName = "notExists",
                Password = "notExists@password"
            });

            Assert.True(false);
        }

        [Fact]
        async public void LogoutALoggedUser()
        {
            var userManager = FakeUserManager.Mock();
            FakeUserManager.FillWithMockUsers(userManager);
            var signInManager = FakeSignInManager.Mock(userManager);
            var jwtIssuerOptions = new MockJwtAuthenticationOptions().Mock();
            var jwtTokenEncoder = new JwtTokenEncoder<GlobeUser>(userManager, jwtIssuerOptions);

            var loginService = new LoginService(userManager, signInManager, jwtIssuerOptions, jwtTokenEncoder);
            await loginService.LoginAsync(new Credentials
            {
                UserName = "user2",
                Password = "user2@passwordNotValid"
            });

            Assert.True(false);
        }
    }
}
