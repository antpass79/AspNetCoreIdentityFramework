using AutoMapper;
using Globe.BusinessLogic.Repositories;
using Globe.Identity.AdministrativeDashboard.Server.Models;
using Globe.Identity.AdministrativeDashboard.Server.Services;
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
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        async public Task<IEnumerable<ApplicationRoleDTO>> Get()
        {
            return await Task.FromResult(_roleService.Get());
        }

        [HttpGet("{roleId}")]
        async public Task<ApplicationRoleDTO> Get(string roleId)
        {
            return await Task.FromResult(_roleService.FindById(roleId));
        }

        [HttpPost]
        async public Task Post([FromBody] ApplicationRoleDTO role)
        {
            _roleService.Insert(role);
            await Task.CompletedTask;
        }

        [HttpPut]
        async public Task Put([FromBody] ApplicationRoleDTO role)
        {
            _roleService.Update(role);
            await Task.CompletedTask;
        }

        [HttpDelete("{roleId}")]
        async public Task Delete(string roleId)
        {
            _roleService.Delete(roleId);
            await Task.CompletedTask;
        }
    }
}
