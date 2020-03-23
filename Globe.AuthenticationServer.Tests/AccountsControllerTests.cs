using Globe.Identity.Authentication.Models;
using Globe.Tests.Web;
using System;
using Xunit;

namespace Globe.AuthenticationServer.Tests
{
    public class AccountsControllerTests
    {
        WebProxyBuilder<Startup> _webProxyBuilder = new WebProxyBuilder<Startup>();

        public AccountsControllerTests()
        {
            _webProxyBuilder.JsonFile("appsettings.json");
            _webProxyBuilder.BaseAddress("https://localhost:44349/api/local/accounts/");
        }

        [Fact]
        async public void RegisterValid()
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

                Assert.True(registrationResult.IsSuccessStatusCode);
            }
        }

        [Fact]
        async public void RegisterInvalidEmptyPassword()
        {
            using (var client = _webProxyBuilder.Build())
            {
                var exception = await Assert.ThrowsAsync<ArgumentException>(() => client.PostAsync("register", new Registration
                {
                    UserName = "antpass79",
                    FirstName = "anto",
                    LastName = "passa",
                    Email = "anto.passa@myemail.it"
                }));

                Assert.Equal("registration", exception.ParamName);
            }
        }
    }
}
