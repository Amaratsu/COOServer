using COO.DataAccess.Contexts;
using COO.Domain.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace COO.Business.Logic.MMO.Write.CreateActiveLogin
{
    public class CreateActiveLoginCommandHandler : IRequestHandler<CreateActiveLoginCommand, int>
    {
        private readonly IDbContextFactory<COODbContext> contextFactory;

        public CreateActiveLoginCommandHandler(IDbContextFactory<COODbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public async Task<int> Handle(CreateActiveLoginCommand request, CancellationToken cancellationToken)
        {
            await using var context = contextFactory.CreateDbContext();
            var activeLogin = new ActiveLogin
            {
                UserId = request.UserId,
                SessionKey = request.SessionKey,
                CharacterId = request.CharacterId
            };

            await context.ActiveLogins.AddAsync(activeLogin, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return activeLogin.Id;
        }
    }
}