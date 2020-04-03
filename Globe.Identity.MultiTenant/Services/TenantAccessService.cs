using Globe.Identity.MultiTenant.Stores;
using Globe.Identity.MultiTenant.Strategies;
using System.Threading.Tasks;

namespace Globe.Identity.MultiTenant.Services
{
    internal class TenantAccessService<T>
        where T: Tenant
    {
        private readonly ITenantResolutionStrategy _tenantResolutionStrategy;
        private readonly ITenantStore<T> _tenantStore;

        public TenantAccessService(ITenantResolutionStrategy tenantResolutionStrategy, ITenantStore<T> tenantStore)
        {
            _tenantResolutionStrategy = tenantResolutionStrategy;
            _tenantStore = tenantStore;
        }

        async public Task<T> GetTenantAsync()
        {
            var tenantIdentifier = await _tenantResolutionStrategy.GetTenantIdentifierAsync();
            return await _tenantStore.GetTenantAsync(tenantIdentifier);
        }
    }
}
