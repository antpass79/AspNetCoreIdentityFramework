using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Globe.Identity.Authentication.Extensions;
using Globe.Identity.Authentication.MultiTenant;
using Globe.Identity.Authentication.MultiTenant.Extensions;
using Globe.Identity.Authentication.MultiTenant.Stores;
using Globe.Identity.Authentication.MultiTenant.Strategies;
using Globe.Identity.Shared.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Globe.MultiTenant.AuthenticationServer
{
    public class Startup
    {
        static IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMultiTenancy()
                .WithResolutionStrategy<HostResolutionStrategy>()
                .WithStore<InMemoryTenantStore>();

            services
                .AddControllers()
                .AddGlobeFluentValidation();
        }

        public static void ConfigureMultiTenantServices(Tenant tenant, ContainerBuilder containerBuilder)
        {
            ServiceCollection tenantServices = new ServiceCollection();

            tenantServices
                .ConfigureGlobeOptions(_configuration)
                .InjectGlobeDependencies()
                .AddGlobeMiddlewares();

            containerBuilder.Populate(tenantServices);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app
                .UseMultiTenancy()
                .UseMultiTenantContainer()
                .UseMultiTenantAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
