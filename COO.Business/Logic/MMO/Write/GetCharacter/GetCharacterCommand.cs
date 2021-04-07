using MediatR;

namespace COO.Business.Logic.MMO.Write.GetCharacter
{
    public sealed class GetCharacterCommand : IRequest<GetCharacterResponseModel>
    {
        public GetCharacterCommand(int userId, string token, int characterId)
        {
            UserId = userId;
            Token = token;
            CharacterId = characterId;
        }

        public int UserId { get; set; }
        public string Token { get; set; }
        public int CharacterId { get; set; }
    }
}
