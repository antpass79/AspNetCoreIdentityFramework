using Microsoft.Extensions.DependencyInjection;
using Globe.Identity.MultiTenant.Builders;

namespace Globe.Identity.MultiTenant.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static TenantBuilder<T> AddMultiTenancy<T>(this IServiceCollection services)
            where T : Tenant => new TenantBuilder<T>(services);

        public static TenantBuilder<Tenant> AddMultiTenancy(this IServiceCollection services)
            => new TenantBuilder<Tenant>(services);
    }
}
