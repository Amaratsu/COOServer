
using MediatR;

namespace COO.Business.Logic.MMO.Read.GetClanCharacters
{
    public sealed class GetClanCharactersQuery : IRequest<GetClanCharactersResponseModel>
    {
        public GetClanCharactersQuery(int userId, int clanId, int characterId)
        {
            UserId = userId;
            ClanId = clanId;
            CharacterId = characterId;
        }

        public int UserId { get; }
        public int ClanId { get; }
        public int CharacterId { get; }
    }
}
