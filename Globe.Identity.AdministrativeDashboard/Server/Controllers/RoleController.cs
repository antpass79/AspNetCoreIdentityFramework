using AutoMapper;
using Globe.BusinessLogic.Repositories;
using Globe.Identity.AdministrativeDashboard.Server.Models;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.Controllers
{
    [Route("api/[controller]")]
    public class RoleController : Controller
    {
        IMapper _mapper;
        private readonly IRepository<ApplicationRole, string> _roleRepository;

        public RoleController(IMapper mapper, IRepository<ApplicationRole, string> roleRepository)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
        }

        [HttpGet]
        async public Task<IEnumerable<ApplicationRoleDTO>> Get()
        {
            return await Task.FromResult(_mapper.Map<IEnumerable<ApplicationRoleDTO>>(_roleRepository.Get()));
        }

        [HttpGet("{roleId}")]
        async public Task<ApplicationRoleDTO> Get(string roleId)
        {
            return await Task.FromResult(_mapper.Map<ApplicationRoleDTO>(_roleRepository.FindById(roleId)));
        }

        [HttpPost]
        async public Task Post([FromBody] ApplicationRoleDTO role)
        {
            _roleRepository.Insert(_mapper.Map<ApplicationRole>(role));
            await Task.CompletedTask;
        }

        [HttpPut]
        async public Task Put([FromBody] ApplicationRoleDTO role)
        {
            _roleRepository.Update(_mapper.Map<ApplicationRole>(role));
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
