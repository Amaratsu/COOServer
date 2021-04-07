using FluentValidation;

namespace COO.Business.Logic.MMO.Write.GetCharacter
{
    public class GetCharacterCommandValidator : AbstractValidator<GetCharacterCommand>
    {
        public GetCharacterCommandValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.Token).NotNull().NotEmpty();
            RuleFor(x => x.CharacterId).NotNull().NotEmpty();
        }
    }
}
