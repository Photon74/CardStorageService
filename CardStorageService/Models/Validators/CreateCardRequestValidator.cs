using CardStorageService.Models.Requests;
using FluentValidation;

namespace CardStorageService.Models.Validators
{
    public class CreateCardRequestValidator : AbstractValidator<CreateCardRequest>
    {
        public CreateCardRequestValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.CardNo).Length(16, 20).CreditCard();
            RuleFor(x => x.CVV2).Length(3);
            RuleFor(x => x.ExpDate).NotEmpty();
        }
    }
}
