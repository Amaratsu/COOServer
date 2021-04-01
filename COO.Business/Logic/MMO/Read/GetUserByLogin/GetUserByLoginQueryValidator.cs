using COO.Business.Logic.MMO.Read.GetUserByLogin;
using FluentValidation;

namespace COO.Business.Logic.MMO.Write.CreateActiveLogin
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