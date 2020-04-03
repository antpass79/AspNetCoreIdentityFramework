using Globe.Identity.Models;
using Globe.Identity.Server;
using Globe.ResourceServer.DTOs;
using Globe.Tests.Web;
using Globe.Tests.Web.Extensions;
using System.Net;
using Xunit;

namespace Globe.ResourceServer.Tests
{
    public class TranslationStringControllerTests
    {
        WebProxyBuilder<StartupTest> _webProxyAuthenticationServerBuilder = new WebProxyBuilder<StartupTest>();
        WebProxyBuilder<Globe.ResourceServer.Startup> _webProxyResourceServerBuilder = new WebProxyBuilder<Globe.ResourceServer.Startup>();

        public TranslationStringControllerTests()
        {
            _webProxyAuthenticationServerBuilder.JsonFile("appsettings.authenticationserver.json");
            _webProxyAuthenticationServerBuilder.BaseAddress("https://localhost:44349/api/");

            _webProxyResourceServerBuilder.JsonFile("appsettings.resourceserver.json");
            _webProxyResourceServerBuilder.BaseAddress("http://localhost:58842/");
        }

        [Fact]
        async public void GetAdministrativeRightsWithoutAuthentication()
        {
            using (var client = _webProxyResourceServerBuilder.Build())
            {
                var result = await client.GetAsync("TranslationStringTest");

                Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
            }
        }

        [Fact]
        async public void GetAdministrativeRightsWithAuthentication()
        {
            using var identityServerClient = _webProxyAuthenticationServerBuilder.Build();
            using var translationServerClient = _webProxyResourceServerBuilder.Build();

            var registerResult = await identityServerClient.PostAsync("accounts/register", new Registration
            {
                UserName = "antopassa79",
                FirstName = "anto",
                LastName = "passa",
                Email = "anto.passa@myemail.ts",
                Password = "mypassword"
            });

            var loginResult = await (await identityServerClient.PostAsync("access/login", new Credentials
            {
                UserName = "antopassa79",
                Password = "mypassword"
            })).GetValue<LoginResult>();

            var translationStringResult = await translationServerClient.GetAsync("TranslationStringTest", loginResult.Token);
            var translationStrings = await translationStringResult.GetValue<TranslationString[]>();

            Assert.True(loginResult.Successful);
            Assert.Equal(HttpStatusCode.OK, translationStringResult.StatusCode);
            Assert.NotEqual(string.Empty, loginResult.Token);
            Assert.Equal(3, translationStrings.Length);
        }

        [Fact]
        async public void GetGuestRightsWithoutAuthentication()
        {
            using (var client = _webProxyResourceServerBuilder.Build())
            {
                var result = await client.GetAsync("TranslationStringTest/getfirst");

                Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
            }
        }

        [Fact]
        async public void GetGuestRightsWithAuthentication()
        {
            using var identityServerClient = _webProxyAuthenticationServerBuilder.Build();
            using var translationServerClient = _webProxyResourceServerBuilder.Build();

            var registerResult = await identityServerClient.PostAsync("accounts/register", new Registration
            {
                UserName = "antopassa79",
                FirstName = "anto",
                LastName = "passa",
                Email = "anto.passa@myemail.ts",
                Password = "mypassword"
            });

            var loginResult = await (await identityServerClient.PostAsync("access/login", new Credentials
            {
                UserName = "antopassa79",
                Password = "mypassword"
            })).GetValue<LoginResult>();

            var translationStringResult = await translationServerClient.GetAsync("TranslationStringTest/getfirst", loginResult.Token);
            var translationString = await translationStringResult.GetValue<TranslationString>();

            Assert.Equal(HttpStatusCode.OK, translationStringResult.StatusCode);
            Assert.NotEqual(string.Empty, loginResult.Token);
            Assert.NotEqual(string.Empty, translationString.Key);
            Assert.NotEqual(string.Empty, translationString.Value);
        }
    }
}