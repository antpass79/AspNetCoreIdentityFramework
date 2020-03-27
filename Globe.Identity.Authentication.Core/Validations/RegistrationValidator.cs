using FluentValidation;
using Globe.Identity.Authentication.Core.Models;

namespace Globe.Identity.Authentication.Core.Validations
{
    public class RegistrationValidator<T> : AbstractValidator<T>
        where T : Registration
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
