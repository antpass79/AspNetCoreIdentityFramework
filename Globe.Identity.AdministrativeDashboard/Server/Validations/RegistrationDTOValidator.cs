using FluentValidation;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using Globe.Identity.Authentication.Core.Validations;

namespace Globe.AuthenticationServer
{
    public class RegistrationDTOValidator : RegistrationValidator<RegistrationDTO>
    {
        public RegistrationDTOValidator()
        {
            RuleFor(model => model.ConfirmPassword)
                .Equal(model => model.Password);
        }
    }
}
