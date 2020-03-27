using Globe.Identity.Authentication.Core.Models;
using Globe.Identity.Authentication.Core.Services;
using Globe.Identity.Authentication.Jwt;
using Globe.Identity.Authentication.Models;
using Globe.Identity.Shared.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Globe.Identity.Authentication.Services
{
    public class LoginService<TUser> : IAsyncLoginService
        where TUser : GlobeUser, new()
    {
        private readonly UserManager<TUser> _userManager;
        private readonly SignInManager<TUser> _signInManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly JwtAuthenticationOptions _jwtOptions;
        IJwtTokenEncoder<TUser> _jwtTokenEncoder;

        public LoginService(UserManager<TUser> userManager, SignInManager<TUser> signInManager, IJwtGenerator jwtGenerator, IOptions<JwtAuthenticationOptions> jwtOptions, IJwtTokenEncoder<TUser> jwtTokenEncoder)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerator = jwtGenerator;
            _jwtOptions = jwtOptions.Value;
            _jwtTokenEncoder = jwtTokenEncoder;
        }

        async public Task<LoginResult> LoginAsync(Credentials credentials)
        {
            if (string.IsNullOrEmpty(credentials.UserName) || string.IsNullOrEmpty(credentials.Password))
                return await Task.FromResult<LoginResult>(new LoginResult
                {
                    Successful = false,
                    Error = "Invalidcredentials"
                });

            var userToVerify = await _userManager.FindByNameAsync(credentials.UserName);
            if (userToVerify == null)
                return await Task.FromResult<LoginResult>(new LoginResult
                {
                    Successful = false,
                    Error = "Invalidcredentials"
                });

            return await Task.FromResult<LoginResult>(new LoginResult
            {
                Successful = true,
                Token = await _jwtTokenEncoder.EncodeAsync(userToVerify)
            });

            //var identity = await GenerateClaimsIdentity(credentials.UserName, credentials.Password);
            //if (identity == null)
            //{
            //    return new LoginResult
            //    {
            //        Successful = false,
            //        Error = "Invalid credentials"
            //    };
            //}

            //return new LoginResult
            //{
            //    Successful = true,
            //    Token = await JwtSerializer.Serialize(identity, _jwtGenerator, credentials.UserName, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented })
            //};
        }

        async public Task LogoutAsync(Credentials credentials)
        {
            await Task.Run(() => throw new NotImplementedException());
        }

        async private Task<LoginResult> GenerateJwtToken(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<LoginResult>(new LoginResult
                {
                    Successful = false,
                    Error = "Invalidcredentials"
                });

            var userToVerify = await _userManager.FindByNameAsync(userName);
            if (userToVerify == null)
                return await Task.FromResult<LoginResult>(new LoginResult
                {
                    Successful = false,
                    Error = "Invalidcredentials"
                });

            return await Task.FromResult<LoginResult>(new LoginResult
            {
                Successful = true,
                Token = await _jwtTokenEncoder.EncodeAsync(userToVerify)
            });
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

    public class LoginService : LoginService<GlobeUser>
    {
        public LoginService(UserManager<GlobeUser> userManager, SignInManager<GlobeUser> signInManager, IJwtGenerator jwtGenerator, IOptions<JwtAuthenticationOptions> jwtOptions, IJwtTokenEncoder<GlobeUser> jwtTokenEncoder)
            : base(userManager, signInManager, jwtGenerator, jwtOptions, jwtTokenEncoder)
        {
        }
    }
}
