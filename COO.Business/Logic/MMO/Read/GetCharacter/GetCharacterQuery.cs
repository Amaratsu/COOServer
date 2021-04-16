using MediatR;

namespace COO.Business.Logic.MMO.Read.GetCharacter
{
    public sealed class GetCharacterQuery : IRequest<GetCharacterResponseModel>
    {
        public GetCharacterQuery(int characterId, int serverId)
        {
            CharacterId = characterId;
            ServerId = serverId;
        }

        public int CharacterId { get; }
        public int ServerId { get; }
    }
}
