using Globe.Identity.Authentication.Models;
using Globe.Identity.Authentication.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.AuthenticationServer.MultiTenant.Controllers
{
    [Route("/api/{tenant}/accounts")]
    public class AccountsController : Controller
    {
        private readonly IAccountsService _accountsService;

        public AccountsController(IAccountsService accountsService)
        {
            this._accountsService = accountsService;
        }

        [HttpPost("register")]
        async public Task<IActionResult> Post([FromBody] Registration registration)
        {
            if (!ModelState.IsValid)
                throw new ArgumentException("Invalid Registration", "registration");

            var identityResult = await _accountsService.Register(registration);
            if (identityResult.Succeeded)
                return Ok();

            this.BuildErrors(identityResult);

            return BadRequest();
        }

        protected void BuildErrors(IdentityResult identityResult)
        {
            identityResult.Errors.ToList().ForEach(error =>
            {
                ModelState.AddModelError(string.Empty, error.Description);
            });
        }
    }
}
