using Globe.Identity.Authentication.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Globe.Identity.Authentication.Data
{
    public class InMemoryGlobeDbContext : GlobeDbContext
    {
        public InMemoryGlobeDbContext(IOptions<DatabaseOptions> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(this.Options.Value.DefaultInMemoryConnection);
        }
    }
}
