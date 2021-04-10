using MediatR;

namespace COO.Business.Logic.MMO.Write.DeleteCharacterFromClan
{
    public sealed class DeleteCharacterFromClanCommand : IRequest<string>
    {
        public DeleteCharacterFromClanCommand(int userId, int characterId)
        {
            UserId = userId;
            CharacterId = characterId;
        }

        public int UserId { get; }
        public int CharacterId { get; }
    }
}
