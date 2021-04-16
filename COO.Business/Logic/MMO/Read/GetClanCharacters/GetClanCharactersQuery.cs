
using MediatR;

namespace COO.Business.Logic.MMO.Read.GetClanCharacters
{
    public sealed class GetClanCharactersQuery : IRequest<GetClanCharactersResponseModel>
    {
        public GetClanCharactersQuery(int characterId)
        {
            CharacterId = characterId;
        }

        public int CharacterId { get; }
    }
}
