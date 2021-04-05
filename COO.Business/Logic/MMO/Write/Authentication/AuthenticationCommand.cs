using MediatR;

namespace COO.Business.Logic.MMO.Write.Authentication
{
    public sealed class AuthenticationCommand : IRequest<AuthenticationResponseModel>
    {
        public AuthenticationCommand(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public string Login { get; }
        public string Password { get; }
    }
}
