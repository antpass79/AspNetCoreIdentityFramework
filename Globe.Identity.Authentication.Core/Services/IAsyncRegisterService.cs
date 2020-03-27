using Globe.Identity.Authentication.Core.Models;
using System.Threading.Tasks;

namespace Globe.Identity.Authentication.Core.Services
{
    public interface IAsyncRegisterService
    {
        Task<RegistrationResult> RegisterAsync(Registration registration);
    }
}
