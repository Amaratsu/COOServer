using MediatR;

namespace COO.Business.Logic.MMO.Write.GetGameServers
{
    public sealed class GetGameServersCommand : IRequest<GetGameServersResponseModel>
    {
        public GetGameServersCommand(int userId, string token)
        {
            UserId = userId;
            Token = token;
        }

        public int UserId { get; }
        public string Token { get; }
    }
}
