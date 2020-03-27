using Globe.Identity.Authentication.Core.Models;

namespace Globe.Identity.AdministrativeDashboard.Shared.DTOs
{
    public class RegistrationDTO : Registration
    {
        public string ConfirmPassword { get; set; }        
    }
}
