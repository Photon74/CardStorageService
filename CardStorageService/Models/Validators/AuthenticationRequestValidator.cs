using CardStorageService.Models.Requests;
using FluentValidation;

namespace CardStorageService.Models.Validators
{
    public class AuthenticationRequestValidator : AbstractValidator<AuthenticationRequest>
    {
        public AuthenticationRequestValidator()
        {
            RuleFor(x => x.Login)
                .NotNull()
                .NotEmpty()
                .Length(8, 255)
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .Length(5, 50);
        }
    }
}
