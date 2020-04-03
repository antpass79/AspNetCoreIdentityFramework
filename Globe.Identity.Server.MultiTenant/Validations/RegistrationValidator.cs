using FluentValidation;
using Globe.Identity.Models;

namespace Globe.Identity.Server.Validations
{
    public class RegistrationValidator : AbstractValidator<Registration>
    {
        public RegistrationValidator()
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
        }
    }
}
