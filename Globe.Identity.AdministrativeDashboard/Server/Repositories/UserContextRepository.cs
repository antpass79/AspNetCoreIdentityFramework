using Globe.BusinessLogic;
using Globe.BusinessLogic.Repositories;
using Globe.Identity.AdministrativeDashboard.Server.Data;
using Globe.Identity.AdministrativeDashboard.Server.Models;
using Globe.Identity.Authentication.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.Repositories
{
    public class UserContextRepository : IAsyncUserRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<ApplicationUser> DbSet => _context.Set<ApplicationUser>();

        public UserContextRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        async public Task<ApplicationUser> FindByIdAsync(string id)
        {
            return await this.DbSet.FindAsync(id);
        }

        async public Task<IEnumerable<ApplicationUser>> GetAsync(Expression<Func<ApplicationUser, bool>> filter = null, Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>> orderBy = null)
        {
            var result = filter != null ? this.DbSet.Where(filter).AsQueryable() : this.DbSet.AsQueryable();
            var ordered =  orderBy != null ? orderBy(result) : result;

            return await ordered.ToListAsync();
        }

        async public Task InsertAsync(ApplicationUser entity)
        {
            await this.DbSet.AddAsync(entity);
        }

        async public Task UpdateAsync(ApplicationUser entity)
        {
            this.DbSet.Update(entity);
            await Task.CompletedTask;
        }

        async public Task DeleteAsync(ApplicationUser entity)
        {
            this.DbSet.Remove(entity);
            await Task.CompletedTask;
        }
    }
}
