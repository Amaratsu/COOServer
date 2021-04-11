using MediatR;

namespace COO.Business.Logic.MMO.Write.DeleteCharacterFromClan
{
    public sealed class DeleteCharacterFromClanCommand : IRequest<string>
    {
        public DeleteCharacterFromClanCommand(int userId, int characterId, string characterName)
        {
            UserId = userId;
            CharacterId = characterId;
            CharacterName = characterName;
        }

        public int UserId { get; }
        public int CharacterId { get; }
        public string CharacterName { get; }
    }
}
