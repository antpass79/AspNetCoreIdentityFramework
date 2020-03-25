using Globe.BusinessLogic;
using Globe.BusinessLogic.Repositories;
using Globe.Identity.AdministrativeDashboard.Server.Models;
using Globe.Identity.Authentication.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Globe.Identity.AdministrativeDashboard.Server.Repositories
{
    public class RoleRepository : IRepository<ApplicationRole, string>, ISaveable
    {
        private readonly GlobeDbContext _context;
        private DbSet<ApplicationRole> DbSet => _context.Set<ApplicationRole>();

        public RoleRepository(GlobeDbContext context)
        {
            _context = context;
        }

        public ApplicationRole FindById(string id)
        {
            return this.DbSet.Find(id);
        }

        public IEnumerable<ApplicationRole> Get(Expression<Func<ApplicationRole, bool>> filter = null, Func<IQueryable<ApplicationRole>, IOrderedQueryable<ApplicationRole>> orderBy = null)
        {
            var result = this.DbSet.Where(filter).AsQueryable();
            return orderBy != null ? orderBy(result).ToList() : result.ToList();
        }

        public void Insert(ApplicationRole entity)
        {
            this.DbSet.Add(entity);
        }

        public void Update(ApplicationRole entity)
        {
            this.DbSet.Update(entity);
        }

        public void Delete(ApplicationRole entity)
        {
            this.DbSet.Remove(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
