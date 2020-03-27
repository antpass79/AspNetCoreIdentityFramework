using Globe.Identity.Authentication.Core.Models;
using Globe.Tests.Web;
using System;
using Xunit;

namespace Globe.AuthenticationServer.Tests
{
    public class AccessControllerTests
    {
        WebProxyBuilder<Startup> _webProxyBuilder = new WebProxyBuilder<Startup>();

        public AccessControllerTests()
        {
            _webProxyBuilder.JsonFile("appsettings.json");
            _webProxyBuilder.BaseAddress("https://localhost:44349/api/local/accounts/");
        }

        [Fact]
        async public void LoginValid()
        {
            using (var client = _webProxyBuilder.Build())
            {
                var registrationResult = await client.PostAsync("register", new Registration
                {
                    UserName = "antpass79",
                    FirstName = "anto",
                    LastName = "passa",
                    Email = "anto.passa@myemail.it",
                    Password = "mypassword"
                });

                var credentialsResult = await client.PostAsync("login", new Credentials
                {
                    UserName = "antpass79",
                    Password = "mypassword"
                });

                Assert.True(registrationResult.IsSuccessStatusCode);
                Assert.True(credentialsResult.IsSuccessStatusCode);
            }
        }

        [Fact]
        async public void LoginInvalidUserName()
        {
            using (var client = _webProxyBuilder.Build())
            {
                await client.PostAsync("register", new Registration
                {
                    UserName = "antpass79",
                    FirstName = "anto",
                    LastName = "passa",
                    Email = "anto.passa@myemail.it",
                    Password = "mypassword"
                });

                var exception = await Assert.ThrowsAsync<ArgumentException>(() => client.PostAsync("login", new Credentials
                {
                    UserName = "antpass791",
                    Password = "mypassword"
                }));

                Assert.Equal("credentials", exception.ParamName);
            }
        }

        [Fact]
        async public void LoginInvalidPassword()
        {
            using (var client = _webProxyBuilder.Build())
            {
                await client.PostAsync("register", new Registration
                {
                    UserName = "antpass79",
                    FirstName = "anto",
                    LastName = "passa",
                    Email = "anto.passa@myemail.it",
                    Password = "mypassword"
                });

                var exception = await Assert.ThrowsAsync<ArgumentException>(() => client.PostAsync("login", new Credentials
                {
                    UserName = "antpass79",
                    Password = "myInvalidpassword"
                }));

                Assert.Equal("credentials", exception.ParamName);
            }
        }
    }
}
