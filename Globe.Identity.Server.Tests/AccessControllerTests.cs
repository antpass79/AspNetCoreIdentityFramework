using Globe.Identity.Models;
using Globe.Identity.Server;
using Globe.Tests.Web;
using System.Net;
using Xunit;

namespace Globe.Identity.Server.Tests
{
    public class AccessControllerTests
    {
        WebProxyBuilder<Startup> _webProxyBuilder = new WebProxyBuilder<Startup>();

        public AccessControllerTests()
        {
            _webProxyBuilder.JsonFile("appsettings.json");
            _webProxyBuilder.BaseAddress("https://localhost:44349/api/");
        }

        [Fact]
        async public void LoginValid()
        {
            using var client = _webProxyBuilder.Build();

            var registrationResult = await client.PostAsync("accounts/register", new Registration
            {
                UserName = "antpass79",
                FirstName = "anto",
                LastName = "passa",
                Email = "anto.passa@myemail.it",
                Password = "mypassword"
            });

            var credentialsResult = await client.PostAsync("access/login", new Credentials
            {
                UserName = "antpass79",
                Password = "mypassword"
            });

            Assert.True(registrationResult.IsSuccessStatusCode);
            Assert.True(credentialsResult.IsSuccessStatusCode);
        }

        [Fact]
        async public void LoginInvalidUserName()
        {
            using var client = _webProxyBuilder.Build();

            await client.PostAsync("accounts/register", new Registration
            {
                UserName = "antpass79",
                FirstName = "anto",
                LastName = "passa",
                Email = "anto.passa@myemail.it",
                Password = "mypassword"
            });

            var result = await client.PostAsync("access/login", new Credentials
            {
                UserName = "antpass791",
                Password = "mypassword"
            });

            Assert.False(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        async public void LoginInvalidPassword()
        {
            using var client = _webProxyBuilder.Build();

            await client.PostAsync("accounts/register", new Registration
            {
                UserName = "antpass79",
                FirstName = "anto",
                LastName = "passa",
                Email = "anto.passa@myemail.it",
                Password = "mypassword"
            });

           var result = await client.PostAsync("access/login", new Credentials
            {
                UserName = "antpass79",
                Password = "myInvalidpassword"
            });

            Assert.False(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}
