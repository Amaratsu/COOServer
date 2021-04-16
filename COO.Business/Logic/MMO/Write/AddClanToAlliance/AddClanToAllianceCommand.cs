using MediatR;

namespace COO.Business.Logic.MMO.Write.AddClanToAlliance
{
    public sealed class AddClanToAllianceCommand : IRequest<string>
    {
        public AddClanToAllianceCommand(int characterId, string clanName)
        {
            CharacterId = characterId;
            ClanName = clanName;
        }

        public int CharacterId { get; }
        public string ClanName { get; }
    }
}
