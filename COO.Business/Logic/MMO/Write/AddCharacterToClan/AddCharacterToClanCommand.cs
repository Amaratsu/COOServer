using MediatR;

namespace COO.Business.Logic.MMO.Write.AddCharacterToClan
{
    public sealed class AddCharacterToClanCommand : IRequest<string>
    {
        public AddCharacterToClanCommand(int userId, int clanId, string characterName)
        {
            UserId = userId;
            ClanId = clanId;
            CharacterName = characterName;
        }

        public int UserId { get; }
        public int ClanId { get; }
        public string CharacterName { get; }
    }
}
