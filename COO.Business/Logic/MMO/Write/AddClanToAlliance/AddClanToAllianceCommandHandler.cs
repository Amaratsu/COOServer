using System;
using System.Threading;
using System.Threading.Tasks;
using COO.Business.Logic.Account.Write.UpdateActivity;
using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Write.AddClanToAlliance
{
    public class AddClanToAllianceCommandHandler : IRequestHandler<AddClanToAllianceCommand, string>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;
        private readonly IMediator _mediator;

        public AddClanToAllianceCommandHandler(IDbContextFactory<CooDbContext> contextFactory, IMediator mediator)
        {
            _contextFactory = contextFactory;
            _mediator = mediator;
        }

        public async Task<string> Handle(AddClanToAllianceCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var foundUser =
                await context.Users.FirstOrDefaultAsync(user => user.Id == request.UserId, cancellationToken);

            if (foundUser != null)
            {
                var foundCharacter = await context
                    .Characters
                    .FirstOrDefaultAsync(c => c.Id == request.CharacterId, cancellationToken);

                if (foundCharacter != null)
                {
                    var foundAllianceLeader =
                        await context.Alliances.FirstOrDefaultAsync(a => a.LeaderId == foundCharacter.Id,
                            cancellationToken);

                    if (foundAllianceLeader != null)
                    {
                        if (foundAllianceLeader.CurrentCountClans + 1 > foundAllianceLeader.MaxCountClans)
                        {
                            throw new AppException("The alliance has a maximum number of clans.");
                        }
                        else
                        {
                            var foundClan = await context.Clans.FirstOrDefaultAsync(c => string.Equals(c.Name, request.ClanName, StringComparison.CurrentCultureIgnoreCase),
                                cancellationToken);

                            if (foundClan != null)
                            {
                                if (foundClan.AllianceId != null)
                                {
                                    foundClan.AllianceId = foundAllianceLeader.Id;
                                    context.Clans.Update(foundClan);

                                    foundAllianceLeader.CurrentCountClans++;
                                    context.Alliances.Update(foundAllianceLeader);

                                    await context.SaveChangesAsync(cancellationToken);

                                    await _mediator.Send(new UpdateActivityCommand(request.UserId), cancellationToken);

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
            else
            {
                throw new AppException("You are not logged in.");
            }
        }
    }
}
