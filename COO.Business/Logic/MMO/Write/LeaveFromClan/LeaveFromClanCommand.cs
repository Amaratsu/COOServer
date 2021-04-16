using MediatR;

namespace COO.Business.Logic.MMO.Write.LeaveFromClan
{
    public sealed class LeaveFromClanCommand : IRequest<string>
    {
        public LeaveFromClanCommand(int characterId)
        {
            CharacterId = characterId;
        }

        public int CharacterId { get; }
    }
}
