using Microsoft.AspNetCore.Identity;

namespace Globe.Identity.AdministrativeDashboard.Server.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }
    }
}
