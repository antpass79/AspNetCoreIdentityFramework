using Globe.Identity.Authentication.Options;
using Globe.Identity.Shared.Options;
using Microsoft.Extensions.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Globe.Identity.Authentication.Jwt
{
    public class JwtGenerator: IJwtGenerator
    {
        private readonly JwtAuthenticationOptions _jwtOptions;

        public JwtGenerator(IOptions<JwtAuthenticationOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
        }

        public async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userName)
            };

            //var claims = new[]
            //{
            //     new Claim(JwtRegisteredClaimNames.Sub, userName),
            //     new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
            //     new Claim("administrator", "full_access"),
            //     new Claim("guest", "limited_access")
            //};

            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return await Task.FromResult(encodedJwt);
        }

        public ClaimsIdentity GenerateClaimsIdentity(string userName, string id)
        {
            return new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
            {
                new Claim("id", id),
                new Claim("administrator", "full_access")
            });
        }

        private static void ThrowIfInvalidOptions(JwtAuthenticationOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtAuthenticationOptions.ValidFor));

            if (options.SigningCredentials == null)
                throw new ArgumentNullException(nameof(JwtAuthenticationOptions.SigningCredentials));

            if (options.JtiGenerator == null)
                throw new ArgumentNullException(nameof(JwtAuthenticationOptions.JtiGenerator));
        }
    }
}
