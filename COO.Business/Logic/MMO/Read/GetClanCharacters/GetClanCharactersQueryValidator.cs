
using FluentValidation;

namespace COO.Business.Logic.MMO.Read.GetClanCharacters
{
    public class GetClanCharactersQueryValidator : AbstractValidator<GetClanCharactersQuery>
    {
        public GetClanCharactersQueryValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.ClanId).NotNull().NotEmpty();
            RuleFor(x => x.CharacterId).NotNull().NotEmpty();
        }
    }
}
