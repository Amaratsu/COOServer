using COO.Domain.Core;
using MediatR;

namespace COO.Business.Logic.MMO.Write.CreateConfirmEmail
{
    public sealed class CreateConfirmEmailCommand : IRequest<string>
    {
        public CreateConfirmEmailCommand(int userId, string token)
        {
            UserId = userId;
            Token = token;
        }

        public int UserId { get; }
        public string Token { get; }
    }
}
