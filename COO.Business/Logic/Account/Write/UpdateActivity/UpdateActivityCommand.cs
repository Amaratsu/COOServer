using MediatR;

namespace COO.Business.Logic.Account.Write.UpdateActivity
{
    public sealed class UpdateActivityCommand : IRequest<bool>
    {
        public UpdateActivityCommand(int userId, int? characterId = null)
        {
            UserId = userId;
            CharacterId = characterId;
        }

        public int UserId { get; }
        public int? CharacterId { get; }
    }
}
