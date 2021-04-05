using MediatR;

namespace COO.Business.Logic.MMO.Write.Registration
{
    public sealed class RegistrationCommand : IRequest<RegistrationResponseModel>
    {
        public RegistrationCommand(string login, string email, string password)
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
