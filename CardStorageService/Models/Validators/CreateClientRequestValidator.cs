using CardStorageService.Models.Requests;
using FluentValidation;

namespace CardStorageService.Models.Validators
{
    public class CreateClientRequestValidator : AbstractValidator<CreateClientRequest>
    {
        public CreateClientRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().Length(3, 255);
            RuleFor(x => x.LastName).NotEmpty().Length(3, 255);
            RuleFor(x => x.Patronymic).MaximumLength(255);
        }
    }
}
