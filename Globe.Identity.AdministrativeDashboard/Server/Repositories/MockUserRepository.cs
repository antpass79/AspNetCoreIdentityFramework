using Globe.BusinessLogic.Repositories;
using Globe.Identity.AdministrativeDashboard.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Globe.Identity.AdministrativeDashboard.Server.Repositories
{
    public class MockUserRepository : IRepository<ApplicationUser, string>
    {
        List<ApplicationUser> users = new List<ApplicationUser>();

        public MockUserRepository()
        {
            for (int i = 0; i < 3; i++)
            {
                users.Add(new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString() + "-" + i.ToString(),
                    FirstName = "First Name " + i.ToString(),
                    LastName = "Last Name " + i.ToString(),
                    UserName = "User Name " + i.ToString(),
                    Email = $"email{i}@email.com"
                });
            }
        }

        public ApplicationUser FindById(string id)
        {
            return users.SingleOrDefault(user => user.Id == id);
        }

        public IEnumerable<ApplicationUser> Get(Expression<Func<ApplicationUser, bool>> filter = null, Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>> orderBy = null)
        {
            var selectedUsers = new List<ApplicationUser>(users);

            if (filter != null)
                selectedUsers = users.Where(user => filter.Compile()(user)).ToList();

            if (orderBy != null)
                selectedUsers = orderBy(users.AsQueryable()).ToList();

            return selectedUsers;
        }

        public void Insert(ApplicationUser entity)
        {
            entity.Id = Guid.NewGuid().ToString() + "-" + users.Count.ToString();
            users.Add(entity);
        }

        public void Update(ApplicationUser entity)
        {
            var user = FindById(entity.Id);
            if (user == null)
                Insert(entity);
            else
            {
                users.Remove(user);
                users.Add(entity);
            }
        }

        public void Delete(ApplicationUser entity)
        {
            var user = FindById(entity.Id);
            users.Remove(user);
        }
    }
}
