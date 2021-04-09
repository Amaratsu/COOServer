using FluentValidation;

namespace COO.Business.Logic.Account.Write.UpdateActivity
{
    public class UpdateActivityCommandValidator : AbstractValidator<UpdateActivityCommand>
    {
        public UpdateActivityCommandValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
        }
    }
}
