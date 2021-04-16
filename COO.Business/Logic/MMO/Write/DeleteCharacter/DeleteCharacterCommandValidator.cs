using FluentValidation;

namespace COO.Business.Logic.MMO.Write.DeleteCharacter
{
    public class DeleteCharacterCommandValidator : AbstractValidator<DeleteCharacterCommand>
    {
        public DeleteCharacterCommandValidator()
        {
            RuleFor(x => x.CharacterId).NotNull().NotEmpty();
        }
    }
}
