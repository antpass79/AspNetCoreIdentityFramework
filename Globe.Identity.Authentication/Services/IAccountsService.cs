using Globe.Identity.Authentication.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Globe.Identity.Authentication.Services
{
    public interface IAccountsService
    {
        Task<IdentityResult> Register(Registration registration);
    }
}
