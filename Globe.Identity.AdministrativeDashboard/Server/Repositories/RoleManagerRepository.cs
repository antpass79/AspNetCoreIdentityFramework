using Globe.BusinessLogic;
using Globe.BusinessLogic.Repositories;
using Globe.Identity.AdministrativeDashboard.Server.Data;
using Globe.Identity.AdministrativeDashboard.Server.Models;
using Globe.Identity.Authentication.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.Repositories
{
    public class RoleManagerRepository : IAsyncRoleRepository
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleManagerRepository(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        async public Task<ApplicationRole> FindByIdAsync(string id)
        {
            return await _roleManager.FindByIdAsync(id);
        }

        async public Task<IEnumerable<ApplicationRole>> GetAsync(Expression<Func<ApplicationRole, bool>> filter = null, Func<IQueryable<ApplicationRole>, IOrderedQueryable<ApplicationRole>> orderBy = null)
        {
            return await _roleManager.Roles.ToListAsync();
        }

        async public Task InsertAsync(ApplicationRole entity)
        {
            await _roleManager.CreateAsync(entity);
        }

        async public Task UpdateAsync(ApplicationRole entity)
        {
            await _roleManager.UpdateAsync(entity);
        }

        async public Task DeleteAsync(ApplicationRole entity)
        {
            await _roleManager.DeleteAsync(entity);
        }
    }
}
