using MediatR;

namespace COO.Business.Logic.MMO.Write.GetCharacters
{
    public sealed class GetCharactersCommand : IRequest<GetCharactersResponseModel>
    {
        public GetCharactersCommand(int userId, string token, int serverId)
        {
            UserId = userId;
            Token = token;
            ServerId = serverId;
        }

        public int UserId { get; }
        public string Token { get; }
        public int ServerId { get; }
    }
}
