namespace Globe.Identity.MultiTenant.Accessors
{
    public interface ITenantAccessor<T>
        where T : Tenant
    {
        T Tenant { get; }
    }
}
