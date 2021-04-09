using MediatR;

namespace COO.Business.Logic.MMO.Write.CreateClan
{
    public sealed class CreateClanCommand : IRequest<string>
    {
        public CreateClanCommand(int userId, int characterId, string clanName)
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
