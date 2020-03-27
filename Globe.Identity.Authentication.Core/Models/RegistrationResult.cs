using System.Collections.Generic;

namespace Globe.Identity.Authentication.Core.Models
{
    public class RegistrationResult
    {
        public bool Successful { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
