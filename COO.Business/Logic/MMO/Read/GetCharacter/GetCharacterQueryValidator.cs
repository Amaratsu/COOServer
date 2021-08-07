using FluentValidation;

namespace COO.Business.Logic.MMO.Read.GetCharacter
{
    public class GetCharacterQueryValidator : AbstractValidator<GetCharacterQuery>
    {
        public GetCharacterQueryValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.CharacterId).NotNull().NotEmpty();
            RuleFor(x => x.ServerId).NotNull().NotEmpty();
        }
    }
}
