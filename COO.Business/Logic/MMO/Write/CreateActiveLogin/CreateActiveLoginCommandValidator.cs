using FluentValidation;

namespace COO.Business.Logic.MMO.Write.CreateActiveLogin
{
    public class CreateActiveLoginCommandValidator : AbstractValidator<CreateActiveLoginCommand>
    {
        public CreateActiveLoginCommandValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.SessionKey).NotNull().NotEmpty();
        }
    }
}