using Globe.Identity.Authentication.Core.Models;
using System.Threading.Tasks;

namespace Globe.Identity.Authentication.Core.Services
{
    public interface IAsyncLoginService
    {
        Task<LoginResult> LoginAsync(Credentials credentials);
        Task LogoutAsync(Credentials credentials);
    }
}
