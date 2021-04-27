using System.Threading;
using System.Threading.Tasks;
using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Write.DeleteClanFromAlliance
{
    public class DeleteClanFromAllianceCommandHandler : IRequestHandler<DeleteClanFromAllianceCommand, string>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;

        public DeleteClanFromAllianceCommandHandler(IDbContextFactory<CooDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<string> Handle(DeleteClanFromAllianceCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var character = await context.Characters
                .FirstOrDefaultAsync(c => c.Id == request.CharacterId, cancellationToken);

            if (character != null)
            {
                var allianceLeaderInAlliance = await context.Alliances.FirstOrDefaultAsync(a => a.LeaderId == character.Id, cancellationToken);
                if (allianceLeaderInAlliance != null)
                {
                    var deleteClanFromAlliance = await context
                        .Clans
                        .FirstOrDefaultAsync(c => c.Name.ToLower() == request.ClanName.ToLower(), cancellationToken);

                    if (deleteClanFromAlliance != null)
                    {
                        if (deleteClanFromAlliance.LeaderId == character.Id)
                        {
                            deleteClanFromAlliance.AllianceId = null;
                            context.Clans.Update(deleteClanFromAlliance);

                            allianceLeaderInAlliance.CurrentCountClans--;
                            context.Alliances.Update(allianceLeaderInAlliance);

                            await context.SaveChangesAsync(cancellationToken);

                            return "OK";
                        }
                        else
                        {
                            throw new AppException("The main clan in an alliance cannot be removed..");
                        }
                    }
                    else
                    {
                        throw new AppException("Only the alliance leader can remove a clan.");
                    }
                }
                else
                {
                    throw new AppException("The character is not the alliance leader.");
                }
            }
            else
            {
                throw new AppException("The character not found.");
            }
        }
    }
}
