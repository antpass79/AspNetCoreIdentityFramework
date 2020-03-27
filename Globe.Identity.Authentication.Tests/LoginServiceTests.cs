using Globe.Identity.Authentication.Core.Models;
using Globe.Identity.Authentication.Jwt;
using Globe.Identity.Authentication.Services;
using Globe.Tests.Mocks;
using System;
using Xunit;

namespace Globe.Identity.Authentication.Tests
{
    public class LoginServiceTests
    {
        [Fact]
        public void LoginWithNotExistsCredentials()
        {
            var userManager = FakeUserManager.Mock();
            var signInManager = FakeSignInManager.Mock(userManager);
            var jwtIssuerOptions = new MockJwtAuthenticationOptions().Mock();
            var jwtGenerator = new JwtGenerator(jwtIssuerOptions);

            var loginService = new LoginService(userManager, signInManager, jwtGenerator, jwtIssuerOptions);
            var exception = Assert.ThrowsAsync<ArgumentException>(() => loginService.LoginAsync(new Credentials
            {
                UserName = "user3",
                Password = "user3@email.com"
            }));

            Assert.Equal("credentials", exception.Result.ParamName);
        }

        [Fact]
        public void LoginWithInvalidCredentials()
        {
            var userManager = FakeUserManager.Mock();
            FakeUserManager.FillWithMockUsers(userManager);
            var signInManager = FakeSignInManager.Mock(userManager);
            var jwtIssuerOptions = new MockJwtAuthenticationOptions().Mock();
            var jwtGenerator = new JwtGenerator(jwtIssuerOptions);

            var loginService = new LoginService(userManager, signInManager, jwtGenerator, jwtIssuerOptions);
            var exception = Assert.ThrowsAsync<ArgumentException>(() => loginService.LoginAsync(new Credentials
            {
                UserName = "user2",
                Password = "user2@passwordNotValid"
            }));

            Assert.Equal("credentials", exception.Result.ParamName);
        }

        [Fact]
        async public void LoginWithValidCredentials()
        {
            var userManager = FakeUserManager.Mock();
            FakeUserManager.FillWithMockUsers(userManager);
            var signInManager = FakeSignInManager.Mock(userManager);
            var jwtIssuerOptions = new MockJwtAuthenticationOptions().Mock();
            var jwtGenerator = new JwtGenerator(jwtIssuerOptions);

            var loginService = new LoginService(userManager, signInManager, jwtGenerator, jwtIssuerOptions);
            var token = await loginService.LoginAsync(new Credentials
            {
                UserName = "user2",
                Password = "user2@password"
            });

            Assert.NotNull(token);
            Assert.NotEmpty(token);
        }

        //[Fact]
        public void LogoutWithNotExistsUser()
        {
            var userManager = FakeUserManager.Mock();
            var signInManager = FakeSignInManager.Mock(userManager);
            var jwtIssuerOptions = new MockJwtAuthenticationOptions().Mock();
            var jwtGenerator = new JwtGenerator(jwtIssuerOptions);

            var loginService = new LoginService(userManager, signInManager, jwtGenerator, jwtIssuerOptions);
            //var exception = Assert.ThrowsAsync<ArgumentException>(() => loginService.Logout(new Credentials
            //{
            //    UserName = "notExists",
            //    Password = "notExists@password"
            //}));

            //Assert.Equal("credentials", exception.Result.ParamName);

            Assert.True(false);
        }

        //[Fact]
        public void LogoutALoggedUser()
        {
            var userManager = FakeUserManager.Mock();
            FakeUserManager.FillWithMockUsers(userManager);
            var signInManager = FakeSignInManager.Mock(userManager);
            var jwtIssuerOptions = new MockJwtAuthenticationOptions().Mock();
            var jwtGenerator = new JwtGenerator(jwtIssuerOptions);

            var loginService = new LoginService(userManager, signInManager, jwtGenerator, jwtIssuerOptions);
            var exception = Assert.ThrowsAsync<ArgumentException>(() => loginService.LoginAsync(new Credentials
            {
                UserName = "user2",
                Password = "user2@passwordNotValid"
            }));

            //var result = await loginService.Logout(new Credentials
            //{
            //    UserName = "user2",
            //    Password = "user2@passwordNotValid"
            //});

            //Assert.Equal("credentials", exception.Result.ParamName);

            Assert.True(false);
        }
    }
}
