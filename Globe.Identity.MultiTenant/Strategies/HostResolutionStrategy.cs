using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Globe.Identity.MultiTenant.Strategies
{
    public class HostResolutionStrategy : ITenantResolutionStrategy
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HostResolutionStrategy(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        async public Task<string> GetTenantIdentifierAsync()
        {
            if (_httpContextAccessor.HttpContext == null)
                return null;

            return await Task.FromResult(_httpContextAccessor.HttpContext.Request.Path.Value);
        }
    }
}
