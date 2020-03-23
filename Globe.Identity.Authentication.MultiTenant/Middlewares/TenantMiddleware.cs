using Microsoft.AspNetCore.Http;
using Globe.Identity.Authentication.MultiTenant.Services;
using System.Threading.Tasks;

namespace Globe.Identity.Authentication.MultiTenant.Middlewares
{
    internal class TenantMiddleware<T>
        where T : Tenant
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (!httpContext.Items.ContainsKey(Constants.HttpContextTenantKey))
            {
                var tenantAccessService = httpContext.RequestServices.GetService(typeof(TenantAccessService<T>)) as TenantAccessService<T>;
                httpContext.Items.Add(Constants.HttpContextTenantKey, await tenantAccessService.GetTenantAsync());
            }

            if (_next != null)
                await _next(httpContext);
        }
    }
}
