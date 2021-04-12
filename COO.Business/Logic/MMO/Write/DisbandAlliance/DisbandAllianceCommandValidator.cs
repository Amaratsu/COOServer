using FluentValidation;

namespace COO.Business.Logic.MMO.Write.DisbandAlliance
{
    public class DisbandAllianceCommandValidator : AbstractValidator<DisbandAllianceCommand>
    {
        public DisbandAllianceCommandValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.CharacterId).NotNull().NotEmpty();
        }
    }
}
