using Globe.Identity.Authentication.Options;
using Globe.Identity.Shared.Options;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Globe.Identity.Authentication.Jwt
{
    public static class JwtSerializer
    {
        public static async Task<string> Serialize(ClaimsIdentity identity, IJwtGenerator jwtGenerator, string userName, JwtAuthenticationOptions jwtOptions, JsonSerializerSettings serializerSettings)
        {
            var response = new
            {
                id = identity.Claims.Single(claim => claim.Type == "id").Value,
                auth_token = await jwtGenerator.GenerateEncodedToken(userName, identity),
                expires_in = (int)jwtOptions.ValidFor.TotalSeconds
            };

            return JsonConvert.SerializeObject(response, serializerSettings);
        }
    }
}
