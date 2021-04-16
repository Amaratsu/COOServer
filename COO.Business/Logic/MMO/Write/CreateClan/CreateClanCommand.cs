using MediatR;

namespace COO.Business.Logic.MMO.Write.CreateClan
{
    public sealed class CreateClanCommand : IRequest<string>
    {
        public CreateClanCommand(int characterId, string clanName)
        {
            CharacterId = characterId;
            ClanName = clanName;
        }

        public int CharacterId { get; }
        public string ClanName { get; }
    }
}
