using System;
using System.Threading;
using System.Threading.Tasks;
using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.Account.Write.UpdateActivity
{
    public class UpdateActivityCommandHandler : IRequestHandler<UpdateActivityCommand, bool>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;

        public UpdateActivityCommandHandler(IDbContextFactory<CooDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<bool> Handle(UpdateActivityCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var user = await context
                .Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user != null)
            {
                user.LastActivity = DateTime.UtcNow;

                context.Users.Update(user);

                if (request.CharacterId != null)
                {
                    var character = await context
                        .Characters
                        .FirstOrDefaultAsync(c => c.Id == request.CharacterId, cancellationToken);

                    if (character != null)
                    {
                        character.IsOnline = true;
                        context.Characters.Update(character);
                    }
                }

                await context.SaveChangesAsync(cancellationToken);

                return true;
            }
            else
            {
                throw new AppException("You are not logged in.");
            }
        }
    }
}
