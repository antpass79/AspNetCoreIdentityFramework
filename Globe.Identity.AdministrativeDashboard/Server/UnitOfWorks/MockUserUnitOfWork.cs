using Globe.BusinessLogic.Repositories;
using Globe.Identity.AdministrativeDashboard.Server.Models;

namespace Globe.Identity.AdministrativeDashboard.Server.UnitOfWorks
{
    public class MockUserUnitOfWork : IUserUnitOfWork
    {
        public IRepository<ApplicationUser, string> UserRepository { get; }

        public MockUserUnitOfWork(IRepository<ApplicationUser, string> userRepository)
        {
            UserRepository = userRepository;
        }

        public void Save()
        {
        }
    }
}
