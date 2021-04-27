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

            var character = await context
                .Characters
                .FirstOrDefaultAsync(c => c.Id == request.CharacterId, cancellationToken);

            var clan = await context.Clans.FirstOrDefaultAsync(c => c.LeaderId == character.Id, cancellationToken);

            if (character != null)
            {

                if (clan != null) {

                    if (character.ClanId != null)
                    {
                        character.ClanId = null;

                        context.Characters.Update(character);

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
    }
}
