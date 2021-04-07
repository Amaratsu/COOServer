using MediatR;

namespace COO.Business.Logic.MMO.Write.GetCharacter
{
    public sealed class GetCharacterCommand : IRequest<GetCharacterResponseModel>
    {
        public GetCharacterCommand(int userId, string token, int characterId, int serverId)
        {
            UserId = userId;
            Token = token;
            CharacterId = characterId;
            ServerId = serverId;
        }

        public int UserId { get; }
        public string Token { get; }
        public int CharacterId { get; }
        public int ServerId { get; }
    }
}
