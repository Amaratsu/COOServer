using FluentValidation;

namespace COO.Business.Logic.MMO.Read.GetUserByLogin
{
    public class GetUserByLoginQueryValidator : AbstractValidator<GetUserByLoginQuery>
    {
        public GetUserByLoginQueryValidator()
        {
            RuleFor(x => x.Login).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty();
        }
    }
}