using FluentValidation;

namespace COO.Business.Logic.MMO.Read.GetGameServers
{
    public class GetGameServersQueryValidator : AbstractValidator<GetGameServersQuery>
    {
        public GetGameServersQueryValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
        }
    }
}
