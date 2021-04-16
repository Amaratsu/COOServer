using MediatR;

namespace COO.Business.Logic.MMO.Read.GetAllianceClans
{
    public sealed class GetAllianceClansQuery : IRequest<GetAllianceClansResponseModel>
    {
        public GetAllianceClansQuery(string allianceName)
        {
            AllianceName = allianceName;
        }

        public string AllianceName { get; set; }
    }
}
