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

        public int UserId { get; }
        public string Token { get; }
        public int CharacterId { get; }
    }
}
