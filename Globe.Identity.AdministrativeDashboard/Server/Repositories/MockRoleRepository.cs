using Globe.BusinessLogic.Repositories;
using Globe.Identity.AdministrativeDashboard.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Globe.Identity.AdministrativeDashboard.Server.Repositories
{
    public class MockRoleRepository : IRepository<ApplicationRole, string>
    {
        List<ApplicationRole> roles = new List<ApplicationRole>();

        public MockRoleRepository()
        {
            for (int i = 0; i < 3; i++)
            {
                roles.Add(new ApplicationRole
                {
                    Id = Guid.NewGuid().ToString() + "-" + i.ToString(),
                    Name = "Role " + i.ToString(),
                    Description = "Description " + i.ToString()
                });
            }
        }

        public ApplicationRole FindById(string id)
        {
            return roles.SingleOrDefault(user => user.Id == id);
        }

        public IEnumerable<ApplicationRole> Get(Expression<Func<ApplicationRole, bool>> filter = null, Func<IQueryable<ApplicationRole>, IOrderedQueryable<ApplicationRole>> orderBy = null)
        {
            var selectedRoles = new List<ApplicationRole>(roles);

            if (filter != null)
                selectedRoles = roles.Where(role => filter.Compile()(role)).ToList();

            if (orderBy != null)
                selectedRoles = orderBy(roles.AsQueryable()).ToList();

            return selectedRoles;
        }

        public void Insert(ApplicationRole entity)
        {
            entity.Id = Guid.NewGuid().ToString() + "-" + roles.Count.ToString();
            roles.Add(entity);
        }

        public void Update(ApplicationRole entity)
        {
            var role = FindById(entity.Id);
            if (role == null)
                Insert(entity);
            else
            {
                roles.Remove(role);
                roles.Add(entity);
            }
        }

        public void Delete(ApplicationRole entity)
        {
            var role = FindById(entity.Id);
            roles.Remove(role);
        }
    }
}
