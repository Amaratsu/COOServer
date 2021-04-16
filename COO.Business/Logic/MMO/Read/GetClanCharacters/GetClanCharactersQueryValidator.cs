
using FluentValidation;

namespace COO.Business.Logic.MMO.Read.GetClanCharacters
{
    public class GetClanCharactersQueryValidator : AbstractValidator<GetClanCharactersQuery>
    {
        public GetClanCharactersQueryValidator()
        {
            RuleFor(x => x.CharacterId).NotNull().NotEmpty();
        }
    }
}
