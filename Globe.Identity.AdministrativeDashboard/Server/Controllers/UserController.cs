using Globe.Identity.AdministrativeDashboard.Server.Services;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        async public Task<IEnumerable<ApplicationUserDTO>> Get()
        {
            return await Task.FromResult(_userService.Get());
        }

        [HttpGet("{userId}")]
        async public Task<UserWithRoles> Get(string userId)
        {
            return await Task.FromResult(_userService.FindById(userId));
        }

        [HttpPost]
        async public Task Post([FromBody] UserWithRoles userWithRoles)
        {
            _userService.Insert(userWithRoles);
            await Task.CompletedTask;
        }

        [HttpPut]
        async public Task Put([FromBody] UserWithRoles userWithRoles)
        {
            _userService.Update(userWithRoles);
            await Task.CompletedTask;
        }

        [HttpDelete("{userId}")]
        async public Task Delete(string userId)
        {
            _userService.Delete(userId);
            await Task.CompletedTask;
        }
    }
}
