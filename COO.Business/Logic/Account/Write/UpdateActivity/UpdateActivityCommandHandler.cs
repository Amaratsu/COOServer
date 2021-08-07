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

            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user != null)
            {
                user.LastActivity = DateTime.UtcNow;

                if (request.CharacterId != null)
                {
                    var character = user.Characters.Find(c => c.Id == request.CharacterId);

                    if (character != null)
                    {
                        character.IsOnline = true;
                    }
                }

                context.Users.Update(user);

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
