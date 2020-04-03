using Microsoft.AspNetCore.Builder;
using Globe.Identity.MultiTenant.Middlewares;

namespace Globe.Identity.MultiTenant.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMultiTenancy<T>(this IApplicationBuilder applicationBuilder)
            where T : Tenant => applicationBuilder.UseMiddleware<TenantMiddleware<T>>();

        public static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder applicationBuilder)
            => applicationBuilder.UseMiddleware<TenantMiddleware<Tenant>>();

        public static IApplicationBuilder UseMultiTenantContainer<T>(this IApplicationBuilder builder) where T : Tenant
            => builder.UseMiddleware<MultiTenantContainerMiddleware<T>>();

        public static IApplicationBuilder UseMultiTenantContainer(this IApplicationBuilder builder)
        => builder.UseMiddleware<MultiTenantContainerMiddleware<Tenant>>();

        public static IApplicationBuilder UseMultiTenantLifescope<T>(this IApplicationBuilder builder) where T : Tenant
            => builder.UseMiddleware<MultiTenantContainerMiddleware<T>>();

        public static IApplicationBuilder UseMultiTenantLifescope(this IApplicationBuilder builder)
            => builder.UseMiddleware<MultiTenantContainerMiddleware<Tenant>>();

        public static IApplicationBuilder UseMultiTenantAuthentication(this IApplicationBuilder builder)
            => builder.UseMiddleware<TenantAuthMiddleware>();
    }
}
