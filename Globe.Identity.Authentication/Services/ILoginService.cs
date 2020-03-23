using Globe.Identity.Authentication.Models;
using System.Threading.Tasks;

namespace Globe.Identity.Authentication.Services
{
    public interface ILoginService
    {
        Task<string> Login(Credentials credentials);
        Task Logout(Credentials credentials);
    }
}
