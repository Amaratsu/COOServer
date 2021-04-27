using System;
using System.Threading;
using System.Threading.Tasks;
using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Write.AddClanToAlliance
{
    public class AddClanToAllianceCommandHandler : IRequestHandler<AddClanToAllianceCommand, string>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;

        public AddClanToAllianceCommandHandler(IDbContextFactory<CooDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<string> Handle(AddClanToAllianceCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var character = await context
                .Characters
                .FirstOrDefaultAsync(c => c.Id == request.CharacterId, cancellationToken);

            if (character != null)
            {
                var allianceLeader =
                    await context.Alliances.FirstOrDefaultAsync(a => a.LeaderId == character.Id,
                        cancellationToken);

                if (allianceLeader != null)
                {
                    if (allianceLeader.CurrentCountClans + 1 > allianceLeader.MaxCountClans)
                    {
                        throw new AppException("The alliance has a maximum number of clans.");
                    }
                    else
                    {
                        var clan = await context.Clans.FirstOrDefaultAsync(c => string.Equals(c.Name, request.ClanName, StringComparison.CurrentCultureIgnoreCase),
                            cancellationToken);

                        if (clan != null)
                        {
                            if (clan.AllianceId != null)
                            {
                                clan.AllianceId = allianceLeader.Id;
                                context.Clans.Update(clan);

                                allianceLeader.CurrentCountClans++;
                                context.Alliances.Update(allianceLeader);

                                await context.SaveChangesAsync(cancellationToken);

                                return "OK";
                            }
                            else
                            {
                                throw new AppException("The clan is already in an alliance.");
                            }
                        }
                        else
                        {
                            throw new AppException("The clan not found.");
                        }
                    }
                }
                else
                {
                    throw new AppException("The character is not an alliance leader.");
                }
            }
            else
            {
                throw new AppException("The character not found.");
            }
        }
    }
}
