using Globe.Identity.Authentication.Core.Models;
using System.Threading.Tasks;

namespace Globe.Identity.Authentication.Core.Services
{
    public interface IAsyncLoginService
    {
        Task<string> LoginAsync(Credentials credentials);
        Task LogoutAsync(Credentials credentials);
    }
}
