using MediatR;

namespace COO.Business.Logic.MMO.Write.CreateActiveLogin
{
    public sealed class CreateActiveLoginCommand : IRequest<int>
    {
        public CreateActiveLoginCommand(int userId, string token)
        {
            UserId = userId;
            Token = token;
        }

        public int UserId { get; }
        public string Token { get; }
    }
}