using MediatR;

namespace COO.Business.Logic.MMO.Read.GetCharacter
{
    public sealed class GetCharacterQuery : IRequest<GetCharacterResponseModel>
    {
        public GetCharacterQuery(int userId, int characterId, int serverId)
        {
            UserId = userId;
            CharacterId = characterId;
            ServerId = serverId;
        }

        public int UserId { get; }
        public int CharacterId { get; }
        public int ServerId { get; }
    }
}
