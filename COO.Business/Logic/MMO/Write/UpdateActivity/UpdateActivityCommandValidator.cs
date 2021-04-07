using FluentValidation;

namespace COO.Business.Logic.MMO.Write.UpdateActivity
{
    public class UpdateActivityCommandValidator : AbstractValidator<UpdateActivityCommand>
    {
        public UpdateActivityCommandValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
        }
    }
}
