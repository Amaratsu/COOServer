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

            var foundCharacter = await context.Characters
                .FirstOrDefaultAsync(character => character.Id == request.CharacterId, cancellationToken);

            var foundClan = await context.Clans.FirstOrDefaultAsync(c => c.LeaderId == foundCharacter.Id, cancellationToken);

            if (foundCharacter != null)
            {

                if (foundClan != null) {

                    if (foundCharacter.ClanId != null)
                    {
                        foundCharacter.ClanId = null;

                        context.Characters.Update(foundCharacter);

                        foundClan.CurrentCountCharacters--;

                        context.Clans.Update(foundClan);

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
