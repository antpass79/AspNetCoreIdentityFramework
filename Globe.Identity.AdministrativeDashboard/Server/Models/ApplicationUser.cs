using Globe.Identity.Authentication.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Globe.Identity.AdministrativeDashboard.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; } = new List<IdentityUserRole<string>>();
    }
}
