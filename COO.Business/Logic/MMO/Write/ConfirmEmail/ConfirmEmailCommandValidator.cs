using FluentValidation;

namespace COO.Business.Logic.MMO.Write.ConfirmEmail
{
    public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.Token).NotNull().NotEmpty();
        }
    }
}
