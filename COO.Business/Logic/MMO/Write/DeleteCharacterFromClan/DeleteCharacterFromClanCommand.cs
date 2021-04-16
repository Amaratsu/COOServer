using MediatR;

namespace COO.Business.Logic.MMO.Write.DeleteCharacterFromClan
{
    public sealed class DeleteCharacterFromClanCommand : IRequest<string>
    {
        public DeleteCharacterFromClanCommand(int characterId, string characterName)
        {
            CharacterId = characterId;
            CharacterName = characterName;
        }

        public int CharacterId { get; }
        public string CharacterName { get; }
    }
}
