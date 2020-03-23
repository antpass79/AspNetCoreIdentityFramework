using Globe.Identity.Authentication.Jwt;
using Globe.Identity.Authentication.Models;
using Globe.Identity.Authentication.Options;
using Globe.Identity.Shared.Models;
using Globe.Identity.Shared.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Globe.Identity.Authentication.Services
{
    public class LoginService : ILoginService
    {
        private readonly UserManager<GlobeUser> _userManager;
        private readonly SignInManager<GlobeUser> _signInManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly JwtAuthenticationOptions _jwtOptions;

        public LoginService(UserManager<GlobeUser> userManager, SignInManager<GlobeUser> signInManager, IJwtGenerator jwtGenerator, IOptions<JwtAuthenticationOptions> jwtOptions)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerator = jwtGenerator;
            _jwtOptions = jwtOptions.Value;
        }


        async public Task<string> Login(Credentials credentials)
        {
            var identity = await GenerateClaimsIdentity(credentials.UserName, credentials.Password);
            if (identity == null)
                throw new ArgumentException("Login failed", "credentials");

            return await JwtSerializer.Serialize(identity, _jwtGenerator, credentials.UserName, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        async public Task Logout(Credentials credentials)
        {
            await Task.Run(() => throw new NotImplementedException());
        }

        private async Task<ClaimsIdentity> GenerateClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            var userToVerify = await _userManager.FindByNameAsync(userName);
            if (userToVerify == null)
                return await Task.FromResult<ClaimsIdentity>(null);

            var result = await _signInManager.PasswordSignInAsync(userToVerify, password, false, false);
            if (result.Succeeded)
                return await Task.FromResult(_jwtGenerator.GenerateClaimsIdentity(userName, userToVerify.Id));

            return await Task.FromResult<ClaimsIdentity>(null);
        }
    }
}
