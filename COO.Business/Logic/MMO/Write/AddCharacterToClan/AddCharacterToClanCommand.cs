using MediatR;

namespace COO.Business.Logic.MMO.Write.AddCharacterToClan
{
    public sealed class AddCharacterToClanCommand : IRequest<string>
    {
        public AddCharacterToClanCommand(int clanId, string characterName)
        {
            ClanId = clanId;
            CharacterName = characterName;
        }

        public int ClanId { get; }
        public string CharacterName { get; }
    }
}
