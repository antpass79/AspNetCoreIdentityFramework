﻿using Globe.Identity.Shared.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Globe.Identity.Authentication.Jwt
{
    public class JwtTokenEncoder<TUser> : IJwtTokenEncoder<TUser>
        where TUser : IdentityUser
    {
        protected UserManager<TUser> UserManager { get; }
        protected IOptions<JwtAuthenticationOptions> Options { get; }
        public JwtTokenEncoder(UserManager<TUser> userManager, IOptions<JwtAuthenticationOptions> options)
        {
            UserManager = userManager;
            Options = options;
        }

        async public Task<string> EncodeAsync(TUser input)
        {
            var claims = await BuildClaimsAsync(input);

            var jwt = new JwtSecurityToken(
                issuer: Options.Value.Issuer,
                audience: Options.Value.Audience,
                claims: claims,
                notBefore: Options.Value.NotBefore,
                expires: Options.Value.Expiration,
                signingCredentials: Options.Value.SigningCredentials);

            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(jwt));
        }

        async virtual protected Task<IEnumerable<Claim>> BuildClaimsAsync(TUser input)
        {
            return await Task.FromResult<IEnumerable<Claim>>(null);
        }
    }
}
