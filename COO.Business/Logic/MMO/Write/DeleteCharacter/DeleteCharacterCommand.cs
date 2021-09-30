using MediatR;

namespace COO.Business.Logic.MMO.Write.DeleteCharacter
{
    public sealed class DeleteCharacterCommand : IRequest<string>
    {
        public DeleteCharacterCommand(int userId, int characterId, int serverId)
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
