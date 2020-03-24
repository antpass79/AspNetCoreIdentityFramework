using Microsoft.AspNet.Identity.EntityFramework;

namespace Globe.Identity.AdministrativeDashboard.Shared.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FullName => $"{LastName} {FirstName}";
    }
}
