using Globe.Identity.Authentication.Data;
using Globe.Identity.Authentication.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Globe.Identity.Authentication.Extensions
{
    public static class EsaoteDbContextExtensions
    {
        public static void AddGlobeDbContext(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            IOptions<DatabaseOptions> databaseOptions = serviceProvider.GetService<IOptions<DatabaseOptions>>();

            _ = databaseOptions.Value.DatabaseType switch
            {
                DatabaseType.Sqlite =>
                    services.AddDbContext<GlobeDbContext, SqliteGlobeDbContext>(),
                DatabaseType.SqlServer =>
                    services.AddDbContext<GlobeDbContext, SqlServerGlobeDbContext>(),
                DatabaseType.InMemory =>
                    services.AddDbContext<GlobeDbContext, InMemoryGlobeDbContext>(
                        ServiceLifetime.Singleton,
                        ServiceLifetime.Singleton),
                _ => throw new NotSupportedException("DbContext not supported")
            };
        }

        public static void Migrate(this GlobeDbContext dbContext)
        {
            dbContext.Database.Migrate();
        }
    }
}
