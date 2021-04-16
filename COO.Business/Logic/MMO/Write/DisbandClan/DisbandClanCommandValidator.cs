using FluentValidation;

namespace COO.Business.Logic.MMO.Write.DisbandClan
{
    public class DisbandClanCommandValidator : AbstractValidator<DisbandClanCommand>
    {
        public DisbandClanCommandValidator()
        {
            RuleFor(x => x.CharacterId).NotNull().NotEmpty();
        }
    }
}
