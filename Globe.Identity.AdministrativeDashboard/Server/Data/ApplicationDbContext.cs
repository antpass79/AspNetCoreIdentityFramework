using Globe.Identity.AdministrativeDashboard.Server.Models;
using Globe.Identity.Authentication.Data;
using Globe.Identity.Authentication.Options;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Globe.Identity.AdministrativeDashboard.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        protected IOptions<DatabaseOptions> Options { get; }
        public ApplicationDbContext(IOptions<DatabaseOptions> options)
        {
            Options = options;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseInMemoryDatabase(this.Options.Value.DefaultInMemoryConnection)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
    }
}
