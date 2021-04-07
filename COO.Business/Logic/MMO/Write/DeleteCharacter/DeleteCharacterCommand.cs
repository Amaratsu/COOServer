using MediatR;

namespace COO.Business.Logic.MMO.Write.DeleteCharacter
{
    public sealed class DeleteCharacterCommand : IRequest<string>
    {
        public DeleteCharacterCommand(int userId, string token, int characterId)
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
