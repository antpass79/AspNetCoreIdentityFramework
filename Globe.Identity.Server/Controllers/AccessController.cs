using Globe.Identity.Models;
using Globe.Identity.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Globe.Identity.Server.Controllers
{
    [Route("/api/access")]
    public class AccessController : Controller
    {
        private readonly IAsyncLoginService _loginService;

        public AccessController(IAsyncLoginService loginService)
        {
            this._loginService = loginService;
        }

        [HttpPost("login")]
        async public Task<IActionResult> Post([FromBody] Credentials credentials)
        {
            if (!ModelState.IsValid)
                throw new ArgumentException("Invalid Credentials", "credentials");

            var result = await _loginService.LoginAsync(credentials);
            if (!result.Successful)
                return BadRequest(result);
            
            return Ok(result);
        }

        [HttpDelete("logout")]
        async public Task<IActionResult> Delete()
        {
            await _loginService.LogoutAsync();
            return Ok();
        }
    }
}
