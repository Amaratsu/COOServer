using FluentValidation;

namespace COO.Business.Logic.MMO.Write.GetGameServers
{
    public class GetGameServersCommandValidator : AbstractValidator<GetGameServersCommand>
    {
        public GetGameServersCommandValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.Token).NotNull().NotEmpty();
        }
    }
}
