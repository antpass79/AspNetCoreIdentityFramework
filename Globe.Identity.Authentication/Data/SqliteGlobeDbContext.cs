using Globe.Identity.Authentication.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Globe.Identity.Authentication.Data
{
    public class SqliteGlobeDbContext : GlobeDbContext
    {
        public SqliteGlobeDbContext(IOptions<DatabaseOptions> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(this.Options.Value.DefaultSqliteConnection);
        }
    }
}
