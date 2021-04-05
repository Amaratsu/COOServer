using FluentValidation;

namespace COO.Business.Logic.MMO.Write.CreateCharacter
{
    public class CreateCharacterCommandValidator : AbstractValidator<CreateCharacterCommand>
    {
        public CreateCharacterCommandValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.Token).NotNull().NotEmpty();
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Gender).NotNull().NotEmpty();
            RuleFor(x => x.RaceId).NotNull().NotEmpty();
            RuleFor(x => x.ClassId).NotNull().NotEmpty();
        }
    }
}
