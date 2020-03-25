using Globe.BusinessLogic;
using Globe.BusinessLogic.Repositories;
using Globe.Identity.AdministrativeDashboard.Server.Models;

namespace Globe.Identity.AdministrativeDashboard.Server.UnitOfWorks
{
    public interface IUserUnitOfWork : ISaveable
    {
        IRepository<ApplicationUser, string> UserRepository { get; }
    }
}
