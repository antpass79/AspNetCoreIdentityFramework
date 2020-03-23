namespace Globe.Identity.Authentication.MultiTenant.Accessors
{
    public interface ITenantAccessor<T>
        where T : Tenant
    {
        T Tenant { get; }
    }
}
