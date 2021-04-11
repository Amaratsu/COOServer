using FluentValidation;

namespace COO.Business.Logic.MMO.Write.CreateAlliance
{
    public class CreateAllianceCommandValidator : AbstractValidator<CreateAllianceCommand>
    {
        public CreateAllianceCommandValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.CharacterId).NotNull().NotEmpty();
            RuleFor(x => x.AllianceName).NotNull().NotEmpty();
        }
    }
}
