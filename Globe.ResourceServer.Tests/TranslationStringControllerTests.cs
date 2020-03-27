using Globe.Identity.Authentication.Core.Models;
using Globe.ResourceServer.DTOs;
using Globe.Tests.Web;
using Globe.Tests.Web.Extensions;
using System.Net;
using Xunit;

namespace Esaote.ResourceServer.Tests
{
    public class TranslationStringControllerTests
    {
        WebProxyBuilder<Globe.AuthenticationServer.Startup> _webProxyAuthenticationServerBuilder = new WebProxyBuilder<Globe.AuthenticationServer.Startup>();
        WebProxyBuilder<Globe.ResourceServer.Startup> _webProxyResourceServerBuilder = new WebProxyBuilder<Globe.ResourceServer.Startup>();

        public TranslationStringControllerTests()
        {
            _webProxyAuthenticationServerBuilder.JsonFile("appsettings.authenticationserver.json");
            _webProxyAuthenticationServerBuilder.BaseAddress("http://localhost:7000/api/accounts/");

            _webProxyResourceServerBuilder.JsonFile("appsettings.resourceserver.json");
            _webProxyResourceServerBuilder.BaseAddress("http://localhost:8000/");
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

            var registerResult = await identityServerClient.PostAsync("register", new Registration
            {
                UserName = "antopassa79",
                FirstName = "anto",
                LastName = "passa",
                Email = "anto.passa@myemail.ts",
                Password = "mypassword"
            });

            var token = await (await identityServerClient.PostAsync("login", new Credentials
            {
                UserName = "antopassa79",
                Password = "mypassword"
            })).GetValue<Token>();

            var translationStringResult = await translationServerClient.GetAsync("TranslationStringTest", token.AccessToken);
            var translationStrings = await translationStringResult.GetValue<TranslationString[]>();

            Assert.Equal(HttpStatusCode.OK, translationStringResult.StatusCode);
            Assert.NotEqual(string.Empty, token.AccessToken);
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

            var registerResult = await identityServerClient.PostAsync("register", new Registration
            {
                UserName = "antopassa79",
                FirstName = "anto",
                LastName = "passa",
                Email = "anto.passa@myemail.ts",
                Password = "mypassword"
            });

            var token = await (await identityServerClient.PostAsync("login", new Credentials
            {
                UserName = "antopassa79",
                Password = "mypassword"
            })).GetValue<Token>();

            var translationStringResult = await translationServerClient.GetAsync("TranslationStringTest/getfirst", token.AccessToken);
            var translationString = await translationStringResult.GetValue<TranslationString>();

            Assert.Equal(HttpStatusCode.OK, translationStringResult.StatusCode);
            Assert.NotEqual(string.Empty, token.AccessToken);
            Assert.NotEqual(string.Empty, translationString.Key);
            Assert.NotEqual(string.Empty, translationString.Value);
        }
    }
}