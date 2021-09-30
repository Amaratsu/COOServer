using System.Threading;
using System.Threading.Tasks;
using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Write.LeaveFromClan
{
    public class LeaveFromClanCommandHandler : IRequestHandler<LeaveFromClanCommand, string>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;

        public LeaveFromClanCommandHandler(IDbContextFactory<CooDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<string> Handle(LeaveFromClanCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user != null)
            {
                var character = user.Characters.Find(c => c.Id == request.CharacterId);

                var clanLeader = await context.Clans.FirstOrDefaultAsync(c => c.LeaderId == character.Id, cancellationToken);

                if (character != null)
                {
                    if (clanLeader != null) {
                        var clan = await context
                            .Clans
                            .FirstOrDefaultAsync(c => c.Characters.Exists(ch => ch.UserId == character.Id), cancellationToken);

                        if (clan != null)
                        {
                            clan.CurrentCountCharacters--;

                            context.Clans.Update(clan);

                            await context.SaveChangesAsync(cancellationToken);

                            return "OK";
                        }
                        else
                        {
                            throw new AppException("The character is not in a clan.");
                        }
                    }
                    else
                    {
                        throw new AppException("Clan leader cannot leave the clan.");
                    }
                }
                else
                {
                    throw new AppException("The character not found.");
                }
            }
            else
            {
                throw new AppException("You are not logged in.");
            }
        }
    }
}
