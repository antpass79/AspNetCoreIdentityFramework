using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Globe.Identity.MultiTenant.Containers;
using System;

namespace Globe.Identity.MultiTenant.Factories
{
    public class MultiTenantServiceProviderFactory<T> : IServiceProviderFactory<ContainerBuilder> where T : Tenant
    {
        public Action<T, ContainerBuilder> _tenantSerivcesConfiguration;

        public MultiTenantServiceProviderFactory(Action<T, ContainerBuilder> tenantSerivcesConfiguration)
        {
            _tenantSerivcesConfiguration = tenantSerivcesConfiguration;
        }

        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            builder.Populate(services);

            return builder;
        }

        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            MultiTenantContainer<T> container = null;

            Func<MultiTenantContainer<T>> containerAccessor = () =>
            {
                return container;
            };

            containerBuilder
                .RegisterInstance(containerAccessor)
                .SingleInstance();

            container = new MultiTenantContainer<T>(containerBuilder.Build(), _tenantSerivcesConfiguration);

            return new AutofacServiceProvider(containerAccessor());
        }
    }
}
