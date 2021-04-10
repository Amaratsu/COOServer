
using MediatR;

namespace COO.Business.Logic.MMO.Read.GetClanCharacters
{
    public sealed class GetClanCharactersCommand : IRequest<GetClanCharactersResponseModel>
    {
        public GetClanCharactersCommand(int userId, int characterId)
        {
            UserId = userId;
            CharacterId = characterId;
        }

        public int UserId { get; set; }
        public int CharacterId { get; }
    }
}
