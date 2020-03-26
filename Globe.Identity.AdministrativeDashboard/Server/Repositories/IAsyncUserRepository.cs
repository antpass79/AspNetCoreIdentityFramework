using Globe.BusinessLogic.Repositories;
using Globe.Identity.AdministrativeDashboard.Server.Models;

namespace Globe.Identity.AdministrativeDashboard.Server.Repositories
{
    public interface IAsyncUserRepository : IAsyncRepository<ApplicationUser, string>
    {
    }
}
