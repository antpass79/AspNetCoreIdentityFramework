using Globe.BusinessLogic.Repositories;
using Globe.Identity.AdministrativeDashboard.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.Controllers
{
    [Route("api/[controller]")]
    public class RoleController : Controller
    {
        private readonly IRepository<ApplicationRole, string> _roleRepository;

        public RoleController(IRepository<ApplicationRole, string> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpGet]
        async public Task<IEnumerable<ApplicationRole>> Get()
        {
            return await Task.FromResult(_roleRepository.Get());
        }

        [HttpGet("{roleId}")]
        async public Task<ApplicationRole> Get(string roleId)
        {
            return await Task.FromResult(_roleRepository.FindById(roleId));
        }

        [HttpPost]
        async public Task Post([FromBody] ApplicationRole role)
        {
            _roleRepository.Insert(role);
            await Task.CompletedTask;
        }

        [HttpPut]
        async public Task Put([FromBody] ApplicationRole role)
        {
            _roleRepository.Update(role);
            await Task.CompletedTask;
        }

        [HttpDelete("{roleId}")]
        async public Task Delete(string roleId)
        {
            var user = _roleRepository.FindById(roleId);
            _roleRepository.Delete(user);
            await Task.CompletedTask;
        }
    }
}
