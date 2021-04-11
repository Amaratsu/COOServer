using MediatR;

namespace COO.Business.Logic.MMO.Write.LeaveFromClan
{
    public sealed class LeaveFromClanCommand : IRequest<string>
    {
        public LeaveFromClanCommand(int userId, int characterId)
        {
            UserId = userId;
            CharacterId = characterId;
        }

        public int UserId { get; }
        public int CharacterId { get; }
    }
}
