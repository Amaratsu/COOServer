using FluentValidation;

namespace COO.Business.Logic.MMO.Read.GetCharacters
{
    public class GetCharactersQueryValidator : AbstractValidator<GetCharactersQuery>
    {
        public GetCharactersQueryValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.ServerId).NotNull().NotEmpty();
        }
    }
}
