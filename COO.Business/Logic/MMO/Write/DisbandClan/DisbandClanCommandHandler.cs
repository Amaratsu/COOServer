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

        public DisbandClanCommandHandler(IDbContextFactory<CooDbContext> contextFactory, IMediator mediator)
        {
            _contextFactory = contextFactory;
        }

        public async Task<string> Handle(DisbandClanCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var foundCharacter = await context.Characters
                .FirstOrDefaultAsync(character => character.Id == request.CharacterId, cancellationToken);

            if (foundCharacter != null)
            {
                if (foundCharacter.ClanId == null)
                {
                    throw new AppException("The Character is not in a clan.");
                }
                else
                {
                    var clan = await context.Clans.FirstOrDefaultAsync(c => c.Id == foundCharacter.ClanId, cancellationToken);

                    if (clan.LeaderId != request.CharacterId)
                    {
                        throw new AppException("The character is not the clan leader.");
                    }
                    else
                    {
                        var foundClanCharacters = await context.Characters.Where(c => c.ClanId == clan.Id)
                            .ToListAsync(cancellationToken);

                        if (foundClanCharacters.Count > 0)
                        {

                            foundClanCharacters.ForEach(fcc => fcc.ClanId = null);

                            context.Characters.UpdateRange(foundClanCharacters);
                        }

                        context.Clans.Remove(clan);

                        foundCharacter.ClanId = null;

                        context.Characters.Update(foundCharacter);

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
