using System.Linq;
using System.Threading.Tasks;

namespace Globe.Identity.Authentication.MultiTenant.Stores
{
    public class InMemoryTenantStore : ITenantStore<Tenant>
    {
        async public Task<Tenant> GetTenantAsync(string identifier)
        {
            var tenant = new[]
            {
                new Tenant
                {
                    Id = "80fdb3c0-5888-4295-bf40-ebee0e3cd8f3",
                    Identifier = "/api/102/accounts"
                },
                new Tenant
                {
                    Id = "90fdb3c0-5888-4295-bf40-ebee0e3cd8f5",
                    Identifier = "/api/101/accounts"
                }

            }.SingleOrDefault(tenant => identifier != null && identifier.StartsWith(tenant.Identifier));

            return await Task.FromResult(tenant);
        }
    }
}
