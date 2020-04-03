using Globe.Identity.MultiTenant.Accessors;
using Globe.Identity.MultiTenant.Options;
using Globe.Identity.MultiTenant.Services;
using Globe.Identity.MultiTenant.Stores;
using Globe.Identity.MultiTenant.Strategies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Globe.Identity.MultiTenant.Builders
{
    public class TenantBuilder<T>
        where T: Tenant
    {
        private readonly IServiceCollection _services;

        public TenantBuilder(IServiceCollection services)
        {
            _services = services;
            _services.AddTransient<TenantAccessService<T>>();
        }

        public TenantBuilder<T> WithResolutionStrategy<V>(ServiceLifetime lifetime = ServiceLifetime.Transient)
            where V : class, ITenantResolutionStrategy
        {
            _services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            _services.TryAddSingleton<ITenantAccessor<T>, TenantAccessor<T>>();
            _services.Add(ServiceDescriptor.Describe(typeof(ITenantResolutionStrategy), typeof(V), lifetime));
            return this;
        }

        public TenantBuilder<T> WithStore<V>(ServiceLifetime lifetime = ServiceLifetime.Transient)
            where V: class, ITenantStore<T>
        {
            _services.Add(ServiceDescriptor.Describe(typeof(ITenantStore<T>), typeof(V), lifetime));
            return this;
        }

        public TenantBuilder<T> WithPerTenantOptions<TOptions>(Action<TOptions, T> tenantConfig) where TOptions : class, new()
        {
            _services.AddSingleton<IOptionsMonitorCache<TOptions>>(a => ActivatorUtilities.CreateInstance<TenantOptionsCache<TOptions, T>>(a));
            _services.AddTransient<IOptionsFactory<TOptions>>(a => ActivatorUtilities.CreateInstance<TenantOptionsFactory<TOptions, T>>(a, tenantConfig));
            _services.AddScoped<IOptionsSnapshot<TOptions>>(a => ActivatorUtilities.CreateInstance<TenantOptions<TOptions>>(a));
            _services.AddSingleton<IOptions<TOptions>>(a => ActivatorUtilities.CreateInstance<TenantOptions<TOptions>>(a));

            return this;
        }
    }
}
