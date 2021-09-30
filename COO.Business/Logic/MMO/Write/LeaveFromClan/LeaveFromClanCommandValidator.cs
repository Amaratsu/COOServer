using FluentValidation;

namespace COO.Business.Logic.MMO.Write.LeaveFromClan
{
    public class LeaveFromClanCommandValidator : AbstractValidator<LeaveFromClanCommand>
    {
        public LeaveFromClanCommandValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.CharacterId).NotNull().NotEmpty();
        }
    }
}
