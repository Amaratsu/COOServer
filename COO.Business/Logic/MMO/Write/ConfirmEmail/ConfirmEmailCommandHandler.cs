using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace COO.Business.Logic.MMO.Write.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, string>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;

        public ConfirmEmailCommandHandler(IDbContextFactory<CooDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<string> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId && u.Token == request.Token, cancellationToken);

            if (user == null)
            {
                throw new AppException("Link is incorrect");
            }

            user.Token = "";
            user.EmailConfirmed = true;
            user.LastActivity = DateTime.UtcNow;

            context.Users.Update(user);
            await context.SaveChangesAsync(cancellationToken);

            return $"User {user.UserName} is confirmed.";
        }
    }
}
