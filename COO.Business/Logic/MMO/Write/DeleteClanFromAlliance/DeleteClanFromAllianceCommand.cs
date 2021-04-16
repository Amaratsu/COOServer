using MediatR;

namespace COO.Business.Logic.MMO.Write.DeleteClanFromAlliance
{
    public sealed class DeleteClanFromAllianceCommand : IRequest<string>
    {
        public DeleteClanFromAllianceCommand(int characterId, string clanName)
        {
            CharacterId = characterId;
            ClanName = clanName;
        }

        public int CharacterId { get; }
        public string ClanName { get; }
    }
}
