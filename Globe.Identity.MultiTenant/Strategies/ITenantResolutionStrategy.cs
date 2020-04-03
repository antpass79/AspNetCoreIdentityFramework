using System.Threading.Tasks;

namespace Globe.Identity.MultiTenant.Strategies
{
    public interface ITenantResolutionStrategy
    {
        Task<string> GetTenantIdentifierAsync();
    }
}
