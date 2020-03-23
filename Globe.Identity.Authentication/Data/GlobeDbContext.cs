using Globe.Identity.Authentication.Models;
using Globe.Identity.Authentication.Options;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Globe.Identity.Authentication.Data
{
    public abstract class GlobeDbContext : IdentityDbContext<GlobeUser>
    {
        protected IOptions<DatabaseOptions> Options { get; }
        protected GlobeDbContext(IOptions<DatabaseOptions> options)
        {
            Options = options;
        }
    }
}
