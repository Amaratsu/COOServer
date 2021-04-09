using MediatR;

namespace COO.Business.Logic.MMO.Write.UpdateActivity
{
    public sealed class UpdateActivityCommand : IRequest<bool>
    {
        public UpdateActivityCommand(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; }
    }
}
