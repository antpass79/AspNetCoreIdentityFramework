using System.Threading.Tasks;

namespace Globe.Identity.Authentication.MultiTenant.Stores
{
    public interface ITenantStore<T>
        where T: Tenant
    {
        Task<T> GetTenantAsync(string identifier);
    }
}
