using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Globe.Identity.AdministrativeDashboard.Server.Services
{
    public interface IRoleService
    {
        ApplicationRoleDTO FindById(string id);
        IEnumerable<ApplicationRoleDTO> Get(Expression<Func<ApplicationRoleDTO, bool>> filter = null, Func<IQueryable<ApplicationRoleDTO>, IOrderedQueryable<ApplicationUserDTO>> orderBy = null);
        void Insert(ApplicationRoleDTO entity);
        void Update(ApplicationRoleDTO entity);
        void Delete(ApplicationRoleDTO entity);
        void Delete(string id);
    }
}
