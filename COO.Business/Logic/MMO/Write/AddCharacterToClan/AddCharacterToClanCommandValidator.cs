using FluentValidation;

namespace COO.Business.Logic.MMO.Write.AddCharacterToClan
{
    public class AddCharacterToClanCommandValidator : AbstractValidator<AddCharacterToClanCommand>
    {
        public AddCharacterToClanCommandValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.ClanId).NotNull().NotEmpty();
            RuleFor(x => x.CharacterName).NotNull().NotEmpty();
        }
    }
}
