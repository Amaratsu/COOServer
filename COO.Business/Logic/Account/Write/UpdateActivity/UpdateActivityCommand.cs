using MediatR;

namespace COO.Business.Logic.Account.Write.UpdateActivity
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
