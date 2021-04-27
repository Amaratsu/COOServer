using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Write.DisbandClan
{
    public class DisbandClanCommandHandler : IRequestHandler<DisbandClanCommand, string>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;

        public DisbandClanCommandHandler(IDbContextFactory<CooDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<string> Handle(DisbandClanCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var character = await context.Characters
                .FirstOrDefaultAsync(c => c.Id == request.CharacterId, cancellationToken);

            if (character != null)
            {
                if (character.ClanId == null)
                {
                    throw new AppException("The Character is not in a clan.");
                }
                else
                {
                    var clan = await context.Clans.FirstOrDefaultAsync(c => c.Id == character.ClanId, cancellationToken);

                    if (clan.LeaderId != request.CharacterId)
                    {
                        throw new AppException("The character is not the clan leader.");
                    }
                    else
                    {
                        var clanCharacters = await context.Characters.Where(c => c.ClanId == clan.Id)
                            .ToListAsync(cancellationToken);

                        if (clanCharacters.Count > 0)
                        {

                            clanCharacters.ForEach(fcc => fcc.ClanId = null);

                            context.Characters.UpdateRange(clanCharacters);
                        }

                        context.Clans.Remove(clan);

                        character.ClanId = null;

                        context.Characters.Update(character);

                        await context.SaveChangesAsync(cancellationToken);

                        return "OK";
                    }
                }
            }
            else
            {
                throw new AppException("The character not found.");
            }
        }
    }
}
