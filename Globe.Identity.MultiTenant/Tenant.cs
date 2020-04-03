using System.Collections.Generic;

namespace Globe.Identity.MultiTenant
{
    public class Tenant
    {
        public string Id { get; set; }
        public string Identifier { get; set; }
        public Dictionary<string, object> Items { get; private set; } = new Dictionary<string, object>();
    }
}
