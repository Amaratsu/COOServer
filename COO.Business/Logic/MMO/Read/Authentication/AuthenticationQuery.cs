using MediatR;

namespace COO.Business.Logic.MMO.Read.Authentication
{
    public sealed class AuthenticationQuery : IRequest<AuthenticationResponseModel>
    {
        public AuthenticationQuery(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public string Login { get; }
        public string Password { get; }
    }
}
