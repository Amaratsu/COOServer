using FluentValidation;

namespace COO.Business.Logic.MMO.Write.GetCharacters
{
    public class GetCharactersCommandValidator : AbstractValidator<GetCharactersCommand>
    {
        public GetCharactersCommandValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.Token).NotNull().NotEmpty();
            RuleFor(x => x.ServerId).NotNull().NotEmpty();
        }
    }
}
