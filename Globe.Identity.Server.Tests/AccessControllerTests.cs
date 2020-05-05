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
                UserName = TestConstants.USER_NAME_antpass79,
                FirstName = TestConstants.FIRST_NAME_anto,
                LastName = TestConstants.LAST_NAME_passa,
                Email = TestConstants.EMAIL_antoNOTpassaETmyemailNOTit,
                Password = TestConstants.PASSWORD_mypassword
            });

            var credentialsResult = await client.PostAsync("access/login", new Credentials
            {
                UserName = TestConstants.USER_NAME_antpass79,
                Password = TestConstants.PASSWORD_mypassword
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
                UserName = TestConstants.USER_NAME_antpass79,
                FirstName = TestConstants.FIRST_NAME_anto,
                LastName = TestConstants.LAST_NAME_passa,
                Email = TestConstants.EMAIL_antoNOTpassaETmyemailNOTit,
                Password = TestConstants.PASSWORD_mypassword
            });

            var result = await client.PostAsync("access/login", new Credentials
            {
                UserName = TestConstants.USER_NAME_antpass791,
                Password = TestConstants.PASSWORD_mypassword
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
                UserName = TestConstants.USER_NAME_antpass79,
                FirstName = TestConstants.FIRST_NAME_anto,
                LastName = TestConstants.LAST_NAME_passa,
                Email = TestConstants.EMAIL_antoNOTpassaETmyemailNOTit,
                Password = TestConstants.PASSWORD_mypassword
            });

           var result = await client.PostAsync("access/login", new Credentials
            {
               UserName = TestConstants.USER_NAME_antpass79,
               Password = TestConstants.PASSWORD_myInvalidpassword
           });

            Assert.False(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        async public void ChangePasswordAndValidLogin()
        {
            using var client = _webProxyBuilder.Build();

            await client.PostAsync("accounts/register", new Registration
            {
                UserName = TestConstants.USER_NAME_antpass79,
                FirstName = TestConstants.FIRST_NAME_anto,
                LastName = TestConstants.LAST_NAME_passa,
                Email = TestConstants.EMAIL_antoNOTpassaETmyemailNOTit,
                Password = TestConstants.PASSWORD_mypassword
            });

            var credentialsResult = await client.PostAsync("access/login", new Credentials
            {
                UserName = TestConstants.USER_NAME_antpass79,
                Password = TestConstants.PASSWORD_mypassword
            });

            var registrationResult = await client.PutAsync("accounts/changePassword", new RegistrationNewPassword
            {
                UserName = TestConstants.USER_NAME_antpass79,
                Password = TestConstants.PASSWORD_mypassword,
                NewPassword = TestConstants.PASSWORD_mynewpassword
            });

            var newCredentialsResult = await client.PostAsync("access/login", new Credentials
            {
                UserName = TestConstants.USER_NAME_antpass79,
                Password = TestConstants.PASSWORD_mynewpassword
            });

            Assert.True(credentialsResult.IsSuccessStatusCode);
            Assert.True(registrationResult.IsSuccessStatusCode);
            Assert.True(newCredentialsResult.IsSuccessStatusCode);
        }

        [Fact]
        async public void ChangePasswordAndInvalidLogin()
        {
            using var client = _webProxyBuilder.Build();

            await client.PostAsync("accounts/register", new Registration
            {
                UserName = TestConstants.USER_NAME_antpass79,
                FirstName = TestConstants.FIRST_NAME_anto,
                LastName = TestConstants.LAST_NAME_passa,
                Email = TestConstants.EMAIL_antoNOTpassaETmyemailNOTit,
                Password = TestConstants.PASSWORD_mypassword
            });

            var credentialsResult = await client.PostAsync("access/login", new Credentials
            {
                UserName = TestConstants.USER_NAME_antpass79,
                Password = TestConstants.PASSWORD_mypassword
            });

            var registrationResult = await client.PutAsync("accounts/changePassword", new RegistrationNewPassword
            {
                UserName = TestConstants.USER_NAME_antpass79,
                Password = TestConstants.PASSWORD_mypassword,
                NewPassword = TestConstants.PASSWORD_mynewpassword
            });

            var newCredentialsResult = await client.PostAsync("access/login", new Credentials
            {
                UserName = TestConstants.USER_NAME_antpass79,
                Password = TestConstants.PASSWORD_mynewpasswordinvalid
            });

            Assert.True(credentialsResult.IsSuccessStatusCode);
            Assert.True(registrationResult.IsSuccessStatusCode);
            Assert.False(newCredentialsResult.IsSuccessStatusCode);
        }
    }
}
