using FluentValidation;

namespace COO.Business.Logic.MMO.Write.AddClanToAlliance
{
    public class AddClanToAllianceCommandValidator : AbstractValidator<AddClanToAllianceCommand>
    {
        public AddClanToAllianceCommandValidator()
        {
            RuleFor(x => x.CharacterId).NotNull().NotEmpty();
            RuleFor(x => x.ClanName).NotNull().NotEmpty();
        }
    }
}
