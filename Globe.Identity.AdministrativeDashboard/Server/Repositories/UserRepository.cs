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
    public class UserRepository : IRepository<ApplicationUser, string>, ISaveable
    {
        private readonly GlobeDbContext _context;
        private DbSet<ApplicationUser> DbSet => _context.Set<ApplicationUser>();

        public UserRepository(GlobeDbContext context)
        {
            _context = context;
        }

        public ApplicationUser FindById(string id)
        {
            return this.DbSet.Find(id);
        }

        public IEnumerable<ApplicationUser> Get(Expression<Func<ApplicationUser, bool>> filter = null, Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>> orderBy = null)
        {
            var result = this.DbSet.Where(filter).AsQueryable();
            return orderBy != null ? orderBy(result).ToList() : result.ToList();
        }

        public void Insert(ApplicationUser entity)
        {
            this.DbSet.Add(entity);
        }

        public void Update(ApplicationUser entity)
        {
            this.DbSet.Update(entity);
        }

        public void Delete(ApplicationUser entity)
        {
            this.DbSet.Remove(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
