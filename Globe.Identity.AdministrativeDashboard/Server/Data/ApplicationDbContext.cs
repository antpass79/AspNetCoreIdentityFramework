using Globe.Identity.Authentication.Data;
using Globe.Identity.Authentication.Options;
using Microsoft.Extensions.Options;

namespace Globe.Identity.AdministrativeDashboard.Server.Data
{
    public class ApplicationDbContext : InMemoryGlobeDbContext
    {
        public ApplicationDbContext(IOptions<DatabaseOptions> options)
            : base(options)
        {
        }
    }
}
