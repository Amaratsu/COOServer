using MediatR;

namespace COO.Business.Logic.MMO.Read.GetAlliances
{
    public sealed class GetAlliancesQuery : IRequest<GetAlliancesResponseModel>
    {
        public GetAlliancesQuery()
        {
        }
    }
}
