using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace Globe.Identity.MultiTenant.Options
{
    public class TenantOptionsCacheDictionary<TOptions> where TOptions : class
    {
        private readonly ConcurrentDictionary<string, IOptionsMonitorCache<TOptions>> _tenantSpecificOptionCaches =
            new ConcurrentDictionary<string, IOptionsMonitorCache<TOptions>>();

        public IOptionsMonitorCache<TOptions> Get(string tenantId)
        {
            return _tenantSpecificOptionCaches.GetOrAdd(tenantId, new OptionsCache<TOptions>());
        }
    }
}
