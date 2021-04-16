using MediatR;

namespace COO.Business.Logic.MMO.Write.DisbandClan
{
    public sealed class DisbandClanCommand : IRequest<string>
    {
        public DisbandClanCommand(int characterId)
        {
            CharacterId = characterId;
        }

        public int CharacterId { get; }
    }
}
