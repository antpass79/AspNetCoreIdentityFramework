using Globe.Identity.Authentication.Core.Models;

namespace Globe.Identity.AdministrativeDashboard.Shared.DTOs
{
    public class CredentialsDTO : Credentials
    {
        public string Email { get; set; }

        public bool RememberMe { get; set; }
    }
}
