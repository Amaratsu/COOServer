using System.Threading;
using System.Threading.Tasks;
using COO.DataAccess.Contexts;
using COO.Domain.Core;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Write.CreateAlliance
{
    public class CreateAllianceCommandHandler : IRequestHandler<CreateAllianceCommand, string>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;

        public CreateAllianceCommandHandler(IDbContextFactory<CooDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<string> Handle(CreateAllianceCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var character = await context
                .Characters
                .FirstOrDefaultAsync(c => c.Id == request.CharacterId, cancellationToken);

            if (character != null)
            {

                var alliance = await context
                    .Alliances
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == request.AllianceName.ToLower(), cancellationToken);

                var clanLeader = await context
                    .Clans
                    .FirstOrDefaultAsync(c => c.LeaderId == request.CharacterId, cancellationToken);

                if (alliance != null)
                {
                    throw new AppException("This alliance name is unavailable.");
                }
                else
                {
                    if (clanLeader != null)
                    {
                        if (clanLeader.AllianceId != null)
                        {
                            throw new AppException("The clan is already in an alliance.");
                        }

                        var newAlliance = new Alliance
                        {
                            LeaderId = character.Id,
                            LeaderName = character.Name,
                            Name = character.Name,
                            CurrentCountClans = 1,
                            MaxCountClans = 3
                        };

                        context.Alliances.Update(newAlliance);

                        await context.SaveChangesAsync(cancellationToken);

                        clanLeader.AllianceId = newAlliance.Id;

                        context.Clans.Update(clanLeader);

                        await context.SaveChangesAsync(cancellationToken);

                        return "OK";
                    }
                    else
                    {
                        throw new AppException("Only the clan leader can create a alliance.");
                    }
                }
            }
            else
            {
                throw new AppException("This character not found.");
            }
        }
    }
}
