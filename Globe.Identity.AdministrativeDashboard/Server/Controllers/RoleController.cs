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
        private readonly IAsyncRoleService _asyncRoleManager;

        public RoleController(IAsyncRoleService asyncRoleManager)
        {
            _asyncRoleManager = asyncRoleManager;
        }

        [HttpGet]
        async public Task<IEnumerable<ApplicationRoleDTO>> Get()
        {
            return await _asyncRoleManager.GetAsync();
        }

        [HttpGet("{roleId}")]
        async public Task<ApplicationRoleDTO> Get(string roleId)
        {
            return await _asyncRoleManager.FindByIdAsync(roleId);
        }

        [HttpPost]
        async public Task Post([FromBody] ApplicationRoleDTO role)
        {
            await _asyncRoleManager.InsertAsync(role);
        }

        [HttpPut]
        async public Task Put([FromBody] ApplicationRoleDTO role)
        {
            await _asyncRoleManager.UpdateAsync(role);
        }

        [HttpDelete("{roleId}")]
        async public Task Delete(string roleId)
        {
            await _asyncRoleManager.DeleteAsync(roleId);
        }
    }
}
