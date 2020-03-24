using Microsoft.AspNet.Identity.EntityFramework;

namespace Globe.Identity.AdministrativeDashboard.Shared.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }
    }
}
