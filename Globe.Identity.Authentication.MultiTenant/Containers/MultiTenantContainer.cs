using Autofac;
using Autofac.Core;
using Autofac.Core.Lifetime;
using Autofac.Core.Resolving;
using Globe.Identity.Authentication.MultiTenant.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Identity.Authentication.MultiTenant.Containers
{
    internal class MultiTenantContainer<T> : IContainer
        where T : Tenant
    {
        private readonly IContainer _applicationContainer;
        private readonly Action<T, ContainerBuilder> _tenantContainerConfiguration;
        private readonly Dictionary<string, ILifetimeScope> _tenantLifetimeScopes = new Dictionary<string, ILifetimeScope>();
        private readonly object _lock = new object();
        private const string _multiTenantTag = "multitenantcontainer";

        public MultiTenantContainer(IContainer applicationContainer, Action<T, ContainerBuilder> containerConfiguration)
        {
            _tenantContainerConfiguration = containerConfiguration;
            _applicationContainer = applicationContainer;
        }

        public IDisposer Disposer => GetCurrentTenantScope().Disposer;

        public object Tag => GetCurrentTenantScope().Tag;

        public IComponentRegistry ComponentRegistry => GetCurrentTenantScope().ComponentRegistry;

        public event EventHandler<LifetimeScopeBeginningEventArgs> ChildLifetimeScopeBeginning;
        public event EventHandler<LifetimeScopeEndingEventArgs> CurrentScopeEnding;
        public event EventHandler<ResolveOperationBeginningEventArgs> ResolveOperationBeginning;

        public ILifetimeScope BeginLifetimeScope()
        {
            return GetCurrentTenantScope().BeginLifetimeScope();
        }

        public ILifetimeScope BeginLifetimeScope(object tag)
        {
            return GetCurrentTenantScope().BeginLifetimeScope(tag);
        }

        public ILifetimeScope BeginLifetimeScope(Action<ContainerBuilder> configurationAction)
        {
            return GetCurrentTenantScope().BeginLifetimeScope(configurationAction);
        }

        public ILifetimeScope BeginLifetimeScope(object tag, Action<ContainerBuilder> configurationAction)
        {
            return GetCurrentTenantScope().BeginLifetimeScope(tag, configurationAction);
        }

        public ValueTask DisposeAsync()
        {
            return GetCurrentTenantScope().DisposeAsync();
        }

        public object ResolveComponent(ResolveRequest request)
        {
            return GetCurrentTenantScope().ResolveComponent(request);
        }

        public ILifetimeScope GetCurrentTenantScope()
        {
            return GetTenantScope(GetCurrentTenant()?.Id);
        }

        public ILifetimeScope GetTenantScope(string tenantId)
        {
            if (tenantId == null)
                return _applicationContainer;

            if (_tenantLifetimeScopes.ContainsKey(tenantId))
                return _tenantLifetimeScopes[tenantId];

            lock (_lock)
            {
                if (_tenantLifetimeScopes.ContainsKey(tenantId))
                {
                    return _tenantLifetimeScopes[tenantId];
                }
                else
                {
                    _tenantLifetimeScopes.Add(tenantId, _applicationContainer.BeginLifetimeScope(_multiTenantTag, a => _tenantContainerConfiguration(GetCurrentTenant(), a)));
                    return _tenantLifetimeScopes[tenantId];
                }
            }
        }

        private T GetCurrentTenant()
        {
            return _applicationContainer.Resolve<TenantAccessService<T>>().GetTenantAsync().GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            lock (_lock)
            {
                foreach (var scope in _tenantLifetimeScopes)
                    scope.Value.Dispose();
                _applicationContainer.Dispose();
            }
        }
    }
}
