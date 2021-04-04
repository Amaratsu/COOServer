using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace COO.Business.Logic.MMO.Write.CreateConfirmEmail
{
    public class CreateConfirmEmailCommandHandler : IRequestHandler<CreateConfirmEmailCommand, string>
    {
        private readonly IDbContextFactory<COODbContext> _contextFactory;

        public CreateConfirmEmailCommandHandler(IDbContextFactory<COODbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<string> Handle(CreateConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var user = await context.Users.FirstOrDefaultAsync(user => user.Id == request.UserId && user.Token == request.Token);

            if (user == null)
            {
                throw new AppException("Link is incorrect");
            }

            user.Token = "";
            user.EmailConfirmed = true;
            user.LastActivity = DateTime.UtcNow;

            context.Users.Update(user);
            await context.SaveChangesAsync(cancellationToken);

            return $"User {user.Name} is confirmed.";
        }
    }
}
