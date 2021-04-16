using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Write.DisbandAlliance
{
    public class DisbandAllianceCommandHandler : IRequestHandler<DisbandAllianceCommand, string>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;

        public DisbandAllianceCommandHandler(IDbContextFactory<CooDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<string> Handle(DisbandAllianceCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var foundCharacter = await context.Characters
                .FirstOrDefaultAsync(character => character.Id == request.CharacterId, cancellationToken);
            if (foundCharacter != null)
            {
                var allianceLeaderInAlliance = await context.Alliances.FirstOrDefaultAsync(a => a.LeaderId == foundCharacter.Id, cancellationToken);
                if (allianceLeaderInAlliance == null)
                {
                    throw new AppException("The character is not the alliance leader.");
                }
                else
                {
                    var foundClans = await context.Clans.Where(c => c.AllianceId == allianceLeaderInAlliance.Id)
                        .ToListAsync(cancellationToken);
                    if (foundClans.Count > 0)
                    {
                        foundClans.ForEach(fc => fc.AllianceId = null);
                        context.Clans.UpdateRange(foundClans);
                    }
                    context.Alliances.Remove(allianceLeaderInAlliance);

                    return "OK";
                }
            }
            else
            {
                throw new AppException("The character not found.");
            }
        }
    }
}
