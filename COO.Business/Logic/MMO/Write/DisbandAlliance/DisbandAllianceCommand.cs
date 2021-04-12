
using MediatR;

namespace COO.Business.Logic.MMO.Write.DisbandAlliance
{
    public sealed class DisbandAllianceCommand : IRequest<string>
    {
        public DisbandAllianceCommand(int userId, int characterId)
        {
            UserId = userId;
            CharacterId = characterId;
        }

        public int UserId { get; }
        public int CharacterId { get; }
    }
}
