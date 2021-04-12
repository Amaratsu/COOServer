using FluentValidation;

namespace COO.Business.Logic.MMO.Write.DeleteClanFromAlliance
{
    public class DeleteClanFromAllianceCommandValidator : AbstractValidator<DeleteClanFromAllianceCommand>
    {
        public DeleteClanFromAllianceCommandValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.CharacterId).NotNull().NotEmpty();
            RuleFor(x => x.ClanName).NotNull().NotEmpty();
        }
    }
}
