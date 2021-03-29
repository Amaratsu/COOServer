using MediatR;

namespace COO.Business.Logic.MMO.Write.CreateActiveLogin
{
    public sealed class CreateActiveLoginCommand : IRequest<int>
    {
        public CreateActiveLoginCommand(int userId, string sessionKey, int? characterId)
        {
            UserId = userId;
            SessionKey = sessionKey;
            CharacterId = characterId;
        }

        public int UserId { get; }
        public string SessionKey { get; }
        public int? CharacterId { get; }
    }
}