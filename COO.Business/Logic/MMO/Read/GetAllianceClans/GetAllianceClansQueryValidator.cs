using FluentValidation;

namespace COO.Business.Logic.MMO.Read.GetAllianceClans
{
    public class GetAllianceClansQueryValidator : AbstractValidator<GetAllianceClansQuery>
    {
        public GetAllianceClansQueryValidator()
        {
            RuleFor(a => a.AllianceName).NotEmpty().NotNull();
        }
    }
}
