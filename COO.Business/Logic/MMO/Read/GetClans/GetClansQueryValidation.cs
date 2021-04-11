using FluentValidation;

namespace COO.Business.Logic.MMO.Read.GetClans
{
    public class GetClansQueryValidation : AbstractValidator<GetClansQuery>
    {
        public GetClansQueryValidation()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
        }
    }
}
