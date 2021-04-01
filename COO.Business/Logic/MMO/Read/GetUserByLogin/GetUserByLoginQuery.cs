using COO.Domain.Core;
using MediatR;

namespace COO.Business.Logic.MMO.Read.GetUserByLogin
{
    public sealed class GetUserByLoginQuery : IRequest<User>
    {
       public GetUserByLoginQuery(string login, string password)
       {
            Login = login;
            Password = password;
       }

        public string Login { get; }
        public string Password { get; }
    }
}