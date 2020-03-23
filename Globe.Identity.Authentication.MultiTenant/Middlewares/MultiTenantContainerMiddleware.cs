using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Globe.Identity.Authentication.MultiTenant.Containers;
using System;
using System.Threading.Tasks;

namespace Globe.Identity.Authentication.MultiTenant.Middlewares
{
    internal class MultiTenantContainerMiddleware<T>
        where T : Tenant
    {
        private readonly RequestDelegate _next;

        public MultiTenantContainerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, Func<MultiTenantContainer<T>> multiTenantContainerAccessor)
        {
            context.RequestServices =
                new AutofacServiceProvider(multiTenantContainerAccessor()
                        .GetCurrentTenantScope().BeginLifetimeScope());

            await _next.Invoke(context);
        }
    }
}
