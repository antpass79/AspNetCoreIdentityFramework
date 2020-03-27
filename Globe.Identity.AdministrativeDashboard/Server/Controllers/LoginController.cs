using AutoMapper;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using Globe.Identity.Authentication.Core.Models;
using Globe.Identity.Authentication.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        readonly IAsyncLoginService _loginService;
        private readonly IMapper _mapper;

        public LoginController(IAsyncLoginService loginService, IMapper mapper)
        {
            _loginService = loginService;
            _mapper = mapper;
        }

        [HttpPost]
        async public Task<LoginResultDTO> Post([FromBody] Credentials credentials)
        {
            if (!ModelState.IsValid)
            {
                return new LoginResultDTO
                {
                    Successful = false,
                    Error = "Invalid credentials fields"
                };
            }

            var mappedCredentials = _mapper.Map<Credentials>(credentials);
            var result = await _loginService.LoginAsync(mappedCredentials);


            return _mapper.Map<LoginResultDTO>(result);
        }
    }
}
