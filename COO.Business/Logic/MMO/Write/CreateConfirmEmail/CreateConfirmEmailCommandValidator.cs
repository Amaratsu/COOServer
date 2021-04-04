using FluentValidation;

namespace COO.Business.Logic.MMO.Write.CreateConfirmEmail
{
    class CreateConfirmEmailCommandValidator : AbstractValidator<CreateConfirmEmailCommand>
    {
        public CreateConfirmEmailCommandValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.Token).NotNull().NotEmpty();
        }
    }
}
