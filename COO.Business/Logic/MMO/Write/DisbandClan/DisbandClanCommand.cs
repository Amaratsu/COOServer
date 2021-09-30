using MediatR;

namespace COO.Business.Logic.MMO.Write.DisbandClan
{
    public sealed class DisbandClanCommand : IRequest<string>
    {
        public DisbandClanCommand(int userId, int characterId)
        {
            UserId = userId;
            CharacterId = characterId;
        }

        public int UserId { get; }
        public int CharacterId { get; }
    }
}
