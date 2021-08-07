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

            //var character = await context.Characters
            //    .FirstOrDefaultAsync(c => c.Id == request.CharacterId, cancellationToken);

            //if (character != null)
            //{
            //    var allianceLeaderInAlliance = await context.Alliances.FirstOrDefaultAsync(a => a.LeaderId == character.Id, cancellationToken);
            //    if (allianceLeaderInAlliance == null)
            //    {
            //        throw new AppException("The character is not the alliance leader.");
            //    }
            //    else
            //    {
            //        var clans = await context.Clans.Where(c => c.AllianceId == allianceLeaderInAlliance.Id)
            //            .ToListAsync(cancellationToken);
            //        if (clans.Count > 0)
            //        {
            //            clans.ForEach(fc => fc.AllianceId = null);
            //            context.Clans.UpdateRange(clans);
            //        }
            //        context.Alliances.Remove(allianceLeaderInAlliance);

            //        return "OK";
            //    }
            //}
            //else
            //{
            //    throw new AppException("The character not found.");
            //}
            return "TODO";
        }
    }
}
