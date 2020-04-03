using FluentValidation;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using Globe.Identity.Validations;

namespace Globe.AuthenticationServer
{
    public class RegistrationDTOValidator : AbstractValidator<RegistrationDTO>
    {
        public RegistrationDTOValidator()
        {
            RuleFor(model => model.UserName)
                .NotEmpty()
                .MaximumLength(25);
            RuleFor(model => model.Password)
                .NotEmpty()
                .MinimumLength(3);
            RuleFor(model => model.Email)
                .EmailAddress();
            RuleFor(model => model.FirstName)
                .NotEmpty();
            RuleFor(model => model.LastName)
                .NotEmpty();

            RuleFor(model => model.ConfirmPassword)
                .Equal(model => model.Password);
        }
    }
}
