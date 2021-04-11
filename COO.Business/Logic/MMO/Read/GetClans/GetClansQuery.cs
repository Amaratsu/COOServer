using MediatR;

namespace COO.Business.Logic.MMO.Read.GetClans
{
    public sealed class GetClansQuery : IRequest<GetClansResponseModel>
    {
        public GetClansQuery(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; }
    }
}
