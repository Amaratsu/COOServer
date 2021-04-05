using MediatR;

namespace COO.Business.Logic.MMO.Write.ConfirmEmail
{
    public sealed class ConfirmEmailCommand : IRequest<string>
    {
        public ConfirmEmailCommand(int userId, string token)
        {
            UserId = userId;
            Token = token;
        }

        public int UserId { get; }
        public string Token { get; }
    }
}
