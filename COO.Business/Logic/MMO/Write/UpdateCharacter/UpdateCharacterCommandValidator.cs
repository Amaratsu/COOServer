using FluentValidation;

namespace COO.Business.Logic.MMO.Write.UpdateCharacter
{
    public class UpdateCharacterCommandValidator : AbstractValidator<UpdateCharacterCommand>
    {
        public UpdateCharacterCommandValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.Health).NotNull().NotEmpty();
            RuleFor(x => x.Mana).NotNull().NotEmpty();
            RuleFor(x => x.Level).NotNull().NotEmpty();
            RuleFor(x => x.Experience).NotNull().NotEmpty();
            RuleFor(x => x.ClassId).NotNull().NotEmpty();
            RuleFor(x => x.PosX).NotNull().NotEmpty();
            RuleFor(x => x.PosY).NotNull().NotEmpty();
            RuleFor(x => x.PosZ).NotNull().NotEmpty();
            RuleFor(x => x.RotationYaw).NotNull().NotEmpty();
        }
    }
}