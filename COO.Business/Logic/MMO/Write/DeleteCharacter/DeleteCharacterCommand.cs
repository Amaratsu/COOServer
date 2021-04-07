using MediatR;

namespace COO.Business.Logic.MMO.Write.DeleteCharacter
{
    public sealed class DeleteCharacterCommand : IRequest<string>
    {
        public DeleteCharacterCommand(int userId, int characterId)
        {
            UserId = userId;
            CharacterId = characterId;
        }

        public int UserId { get; }
        public int CharacterId { get; }
    }
}
