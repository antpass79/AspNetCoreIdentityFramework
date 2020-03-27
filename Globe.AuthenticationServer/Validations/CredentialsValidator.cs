using FluentValidation;
using Globe.Identity.Authentication.Core.Models;

namespace Globe.AuthenticationServer
{
    public class CredentialsValidator : AbstractValidator<Credentials>
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
