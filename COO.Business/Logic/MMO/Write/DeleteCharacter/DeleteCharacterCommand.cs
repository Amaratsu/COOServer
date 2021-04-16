using MediatR;

namespace COO.Business.Logic.MMO.Write.DeleteCharacter
{
    public sealed class DeleteCharacterCommand : IRequest<string>
    {
        public DeleteCharacterCommand(int characterId)
        {
            CharacterId = characterId;
        }

        public int CharacterId { get; }
    }
}
