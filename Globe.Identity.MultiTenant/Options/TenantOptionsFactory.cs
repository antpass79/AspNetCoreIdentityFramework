using Microsoft.Extensions.Options;
using Globe.Identity.MultiTenant.Accessors;
using System;
using System.Collections.Generic;

namespace Globe.Identity.MultiTenant.Options
{
    internal class TenantOptionsFactory<TOptions, T> : IOptionsFactory<TOptions>
        where TOptions : class, new()
        where T : Tenant
    {

        private readonly IEnumerable<IConfigureOptions<TOptions>> _setups;
        private readonly IEnumerable<IPostConfigureOptions<TOptions>> _postConfigures;
        private readonly Action<TOptions, T> _tenantConfig;
        private readonly ITenantAccessor<T> _tenantAccessor;

        public TenantOptionsFactory(
            IEnumerable<IConfigureOptions<TOptions>> setups,
            IEnumerable<IPostConfigureOptions<TOptions>> postConfigures, Action<TOptions, T> tenantConfig, ITenantAccessor<T> tenantAccessor)
        {
            _setups = setups;
            _postConfigures = postConfigures;
            _tenantAccessor = tenantAccessor;
            _tenantConfig = tenantConfig;
        }

        public TOptions Create(string name)
        {
            var options = new TOptions();

            foreach (var setup in _setups)
            {
                if (setup is IConfigureNamedOptions<TOptions> namedSetup)
                {
                    namedSetup.Configure(name, options);
                }
                else
                {
                    setup.Configure(options);
                }
            }

            if (_tenantAccessor.Tenant != null)
                _tenantConfig(options, _tenantAccessor.Tenant);

            foreach (var postConfig in _postConfigures)
            {
                postConfig.PostConfigure(name, options);
            }

            return options;
        }
    }
}