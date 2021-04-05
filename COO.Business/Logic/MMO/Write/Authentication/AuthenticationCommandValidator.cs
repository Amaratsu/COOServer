using FluentValidation;

namespace COO.Business.Logic.MMO.Write.Authentication
{
    public class AuthenticationCommandValidator : AbstractValidator<AuthenticationCommand>
    {
        public AuthenticationCommandValidator()
        {
            RuleFor(x => x.Login).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty();
        }
    }
}
