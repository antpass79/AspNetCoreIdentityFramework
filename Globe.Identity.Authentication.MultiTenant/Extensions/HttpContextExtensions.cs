using Microsoft.AspNetCore.Http;

namespace Globe.Identity.Authentication.MultiTenant.Extensions
{
    public static class HttpContextExtensions
    {
        public static T GetTenant<T>(this HttpContext httpContext)
            where T: Tenant
        {
            if (!httpContext.Items.ContainsKey(Constants.HttpContextTenantKey))
                return null;

            return httpContext.Items[Constants.HttpContextTenantKey] as T;
        }

        public static Tenant GetTenant(this HttpContext httpContext)
        {
            return httpContext.GetTenant<Tenant>();
        }
    }
}
