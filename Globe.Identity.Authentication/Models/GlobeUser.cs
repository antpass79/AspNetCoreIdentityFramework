using Microsoft.AspNetCore.Identity;

namespace Globe.Identity.Authentication.Models
{
    public class GlobeUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
