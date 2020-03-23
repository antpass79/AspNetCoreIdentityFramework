using Globe.Identity.Authentication.Models;
using Globe.Identity.Shared.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Globe.Identity.Authentication.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly UserManager<GlobeUser> _userManager;

        public AccountsService(UserManager<GlobeUser> userManager)
        {
            _userManager = userManager;
        }

        async public Task<IdentityResult> Register(Registration registration)
        {
            var user = new GlobeUser
            {
                FirstName = registration.FirstName,
                LastName = registration.LastName,
                UserName = registration.UserName,
                Email = registration.Email
            };

            var result = await _userManager.CreateAsync(user, registration.Password);
            return await Task.FromResult(result);
        }
    }
}
