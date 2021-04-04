using COO.Domain.Core;
using MediatR;

namespace COO.Business.Logic.MMO.Write.CreateRegister
{
    public sealed class CreateRegisterCommand : IRequest<User>
    {
        public CreateRegisterCommand(string login, string email, string password)
        {
            Login = login;
            Email = email;
            Password = password;
        }

        public string Login { get; }
        public string Email { get; }
        public string Password { get; }
    }
}
