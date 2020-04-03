using Globe.Infrastructure.EFCore.Repositories;
using Globe.TranslationServer.Data;

namespace Globe.TranslationServer.Repositories
{
    public class LocalizableStringRepository : AsyncGenericRepository<LocalizableStringRepository>
    {
        public LocalizableStringRepository(LocalizationDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
