using Globe.BusinessLogic.Repositories;
using Globe.Identity.AdministrativeDashboard.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IRepository<ApplicationUser, string> _userRepository;

        public UserController(IRepository<ApplicationUser, string> userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        async public Task<IEnumerable<ApplicationUser>> Get()
        {
            return await Task.FromResult(_userRepository.Get());
        }

        [HttpGet("{userId}")]
        async public Task<ApplicationUser> Get(string userId)
        {
            return await Task.FromResult(_userRepository.FindById(userId));
        }

        [HttpPost]
        async public Task Post([FromBody] ApplicationUser user)
        {
            _userRepository.Insert(user);
            await Task.CompletedTask;
        }

        [HttpPut]
        async public Task Put([FromBody] ApplicationUser user)
        {
            _userRepository.Update(user);
            await Task.CompletedTask;
        }

        [HttpDelete("{userId}")]
        async public Task Delete(string userId)
        {
            var user = _userRepository.FindById(userId);
            _userRepository.Delete(user);
            await Task.CompletedTask;
        }
    }
}
