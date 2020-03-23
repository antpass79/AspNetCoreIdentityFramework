using System.Security.Claims;
using System.Threading.Tasks;

namespace Globe.Identity.Authentication.Jwt
{
    public interface IJwtGenerator
    {
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);
        ClaimsIdentity GenerateClaimsIdentity(string userName, string id);
    }
}
