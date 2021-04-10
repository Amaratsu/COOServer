
using FluentValidation;

namespace COO.Business.Logic.MMO.Read.GetClanCharacters
{
    public class GetClanCharactersCommandValidator : AbstractValidator<GetClanCharactersCommand>
    {
        public GetClanCharactersCommandValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.CharacterId).NotNull().NotEmpty();
        }
    }
}
