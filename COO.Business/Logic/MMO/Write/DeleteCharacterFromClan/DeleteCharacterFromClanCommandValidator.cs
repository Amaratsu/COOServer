using FluentValidation;

namespace COO.Business.Logic.MMO.Write.DeleteCharacterFromClan
{
    public class DeleteCharacterFromClanCommandValidator : AbstractValidator<DeleteCharacterFromClanCommand>
    {
        public DeleteCharacterFromClanCommandValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.CharacterId).NotNull().NotEmpty();
            RuleFor(x => x.CharacterName).NotNull().NotEmpty();
        }
    }
}
