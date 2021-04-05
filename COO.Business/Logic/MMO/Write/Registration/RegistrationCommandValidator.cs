using FluentValidation;

namespace COO.Business.Logic.MMO.Write.Registration
{
    public class RegistrationCommandValidator : AbstractValidator<RegistrationCommand>
    {
        public RegistrationCommandValidator()
        {
            RuleFor(x => x.Login).NotNull().NotEmpty();
            RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotNull().NotEmpty();
        }
    }
}
