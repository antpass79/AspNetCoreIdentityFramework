﻿using Globe.Identity.Authentication.Core.Models;
using Globe.Identity.Authentication.Models;
using Globe.Identity.Authentication.Core.Services;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.Identity.Authentication.Services
{
    public class RegisterService<TUser> : IAsyncRegisterService
        where TUser : GlobeUser, new()
    {
        private readonly UserManager<TUser> _userManager;

        public RegisterService(UserManager<TUser> userManager)
        {
            _userManager = userManager;
        }

        async public Task<RegistrationResult> RegisterAsync(Registration registration)
        {
            var user = new TUser
            {
                FirstName = registration.FirstName,
                LastName = registration.LastName,
                UserName = registration.UserName,
                Email = registration.Email
            };

            var result = await _userManager.CreateAsync(user, registration.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);
                return new RegistrationResult { Successful = false, Errors = errors };
            }
            else
            {
                return new RegistrationResult { Successful = true };
            }
        }
    }
}
