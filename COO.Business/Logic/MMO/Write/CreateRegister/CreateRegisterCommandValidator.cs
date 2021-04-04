using FluentValidation;

namespace COO.Business.Logic.MMO.Write.CreateRegister
{
    class CreateRegisterCommandValidator : AbstractValidator<CreateRegisterCommand>
    {
        public CreateRegisterCommandValidator()
        {
            RuleFor(x => x.Login).NotNull().NotEmpty();
            RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotNull().NotEmpty();
        }
    }
}
