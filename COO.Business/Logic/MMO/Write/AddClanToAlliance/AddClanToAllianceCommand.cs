using MediatR;

namespace COO.Business.Logic.MMO.Write.AddClanToAlliance
{
    public sealed class AddClanToAllianceCommand : IRequest<string>
    {
        public AddClanToAllianceCommand(int userId, int characterId, string clanName)
        {
            UserId = userId;
            CharacterId = characterId;
            ClanName = clanName;
        }

        public int UserId { get; }
        public int CharacterId { get; }
        public string ClanName { get; }
    }
}
