using FluentValidation;
using Globe.Identity.Authentication.Core.Models;

namespace Globe.Identity.Authentication.Core.Validations
{
    public class CredentialsValidator<T> : AbstractValidator<T>
        where T : Credentials
    {
        public CredentialsValidator()
        {
            RuleFor(model => model.UserName)
                .NotEmpty()
                .MaximumLength(25);
            RuleFor(model => model.Password)
                .NotEmpty()
                .MinimumLength(3);
        }
    }
}
