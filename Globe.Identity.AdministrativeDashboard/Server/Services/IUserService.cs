using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Globe.Identity.AdministrativeDashboard.Server.Services
{
    public interface IUserService
    {
        UserWithRoles FindById(string id);
        IEnumerable<ApplicationUserDTO> Get(Expression<Func<ApplicationUserDTO, bool>> filter = null, Func<IQueryable<ApplicationUserDTO>, IOrderedQueryable<ApplicationUserDTO>> orderBy = null);
        void Insert(UserWithRoles entity);
        void Update(UserWithRoles entity);
        void Delete(ApplicationUserDTO entity);
        void Delete(string id);
    }
}
