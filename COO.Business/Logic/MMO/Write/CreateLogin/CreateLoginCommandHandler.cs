using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace COO.Business.Logic.MMO.Write.CreateLogin
{
    public sealed class CreateLoginCommandHandler : IRequestHandler<CreateLoginCommand, CreateLoginResponseModel>
    {
        public Task<CreateLoginResponseModel> Handle(CreateLoginCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
