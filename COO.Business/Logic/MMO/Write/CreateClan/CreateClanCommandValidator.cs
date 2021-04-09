using FluentValidation;

namespace COO.Business.Logic.MMO.Write.CreateClan
{
    public class CreateClanCommandValidator : AbstractValidator<CreateClanCommand>
    {
        public CreateClanCommandValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.CharacterId).NotNull().NotEmpty();
            RuleFor(x => x.ClanName).NotNull().NotEmpty();
        }
    }
}
