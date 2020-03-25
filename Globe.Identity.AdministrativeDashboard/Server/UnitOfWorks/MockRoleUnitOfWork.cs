using Globe.BusinessLogic.Repositories;
using Globe.Identity.AdministrativeDashboard.Server.Models;

namespace Globe.Identity.AdministrativeDashboard.Server.UnitOfWorks
{
    public class MockRoleUnitOfWork : IRoleUnitOfWork
    {
        public IRepository<ApplicationRole, string> RoleRepository { get; }

        public MockRoleUnitOfWork(IRepository<ApplicationRole, string> roleRepository)
        {
            RoleRepository = roleRepository;
        }

        public void Save()
        {
        }
    }
}
