using MediatR;

namespace COO.Business.Logic.MMO.Read.GetCharacters
{
    public sealed class GetCharactersQuery : IRequest<GetCharactersResponseModel>
    {
        public GetCharactersQuery(int userId, int serverId)
        {
            UserId = userId;
            ServerId = serverId;
        }

        public int UserId { get; }
        public int ServerId { get; }
    }
}
