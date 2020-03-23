using System.Threading.Tasks;

namespace Globe.Identity.Authentication.MultiTenant.Strategies
{
    public interface ITenantResolutionStrategy
    {
        Task<string> GetTenantIdentifierAsync();
    }
}
