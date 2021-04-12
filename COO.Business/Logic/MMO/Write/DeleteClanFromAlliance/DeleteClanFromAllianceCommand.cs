using MediatR;

namespace COO.Business.Logic.MMO.Write.DeleteClanFromAlliance
{
    public sealed class DeleteClanFromAllianceCommand : IRequest<string>
    {
        public DeleteClanFromAllianceCommand(int userId, int characterId, string clanName)
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
