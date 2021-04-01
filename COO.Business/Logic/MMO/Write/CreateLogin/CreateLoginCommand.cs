using MediatR;

namespace COO.Business.Logic.MMO.Write.CreateLogin
{
    public sealed class CreateLoginCommand : IRequest<CreateLoginResponseModel>
    {
        public CreateLoginCommand(string name, string password)
        {
            Name = name;
            Password = password;
        }

        public string Name { get; }
        public string Password { get; }
    }
}
