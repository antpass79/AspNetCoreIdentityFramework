using Globe.Identity.Authentication.Models;
using Globe.Identity.Authentication.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Globe.AuthenticationServer.MultiTenant.Controllers
{
    [Route("/api/{tenant}//access")]
    public class AccessController : Controller
    {
        private readonly ILoginService _loginService;

        public AccessController(ILoginService loginService)
        {
            this._loginService = loginService;
        }

        [HttpPost("login")]
        async public Task<IActionResult> Post([FromBody] Credentials credentials)
        {
            if (!ModelState.IsValid)
                throw new ArgumentException("Invalid Credentials", "credentials");

            var result = await _loginService.Login(credentials);
            return Ok(result);
        }

        [HttpDelete("logout")]
        async public Task<IActionResult> Delete([FromBody] Credentials credentials)
        {
            if (!ModelState.IsValid)
                throw new ArgumentException("Invalid Credentials", "credentials");

            await Task.Run(() => throw new NotImplementedException());
            return Ok();
        }
    }
}
