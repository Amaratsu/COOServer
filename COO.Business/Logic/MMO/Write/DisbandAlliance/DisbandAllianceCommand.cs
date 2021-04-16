
using MediatR;

namespace COO.Business.Logic.MMO.Write.DisbandAlliance
{
    public sealed class DisbandAllianceCommand : IRequest<string>
    {
        public DisbandAllianceCommand(int characterId)
        {
            CharacterId = characterId;
        }

        public int CharacterId { get; }
    }
}
