using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using COO.Business.Logic.Account.Write.UpdateActivity;
using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Write.DisbandAlliance
{
    public class DisbandAllianceCommandHandler : IRequestHandler<DisbandAllianceCommand, string>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;
        private readonly IMediator _mediator;

        public DisbandAllianceCommandHandler(IDbContextFactory<CooDbContext> contextFactory, IMediator mediator)
        {
            _contextFactory = contextFactory;
            _mediator = mediator;
        }

        public async Task<string> Handle(DisbandAllianceCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var foundUser =
                await context.Users.FirstOrDefaultAsync(user => user.Id == request.UserId, cancellationToken);

            if (foundUser != null)
            {
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

                        await _mediator.Send(new UpdateActivityCommand(request.UserId), cancellationToken);

                        return "OK";
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
