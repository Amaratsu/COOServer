using System.Threading;
using System.Threading.Tasks;
using COO.Business.Logic.Account.Write.UpdateActivity;
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
        private readonly IMediator _mediator;

        public CreateAllianceCommandHandler(IDbContextFactory<CooDbContext> contextFactory, IMediator mediator)
        {
            _contextFactory = contextFactory;
            _mediator = mediator;
        }

        public async Task<string> Handle(CreateAllianceCommand request, CancellationToken cancellationToken)
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

                    var foundAlliance = await context
                        .Alliances
                        .FirstOrDefaultAsync(c => c.Name.ToLower() == request.AllianceName.ToLower(), cancellationToken);

                    var foundClanLeader = await context
                        .Clans
                        .FirstOrDefaultAsync(c => c.LeaderId == request.CharacterId, cancellationToken);

                    if (foundAlliance != null)
                    {
                        throw new AppException("This alliance name is unavailable.");
                    }
                    else
                    {
                        if (foundClanLeader != null)
                        {
                            if (foundClanLeader.AllianceId != null)
                            {
                                throw new AppException("The clan is already in an alliance.");
                            }

                            var newAlliance = new Alliance
                            {
                                LeaderId = foundCharacter.Id,
                                Name = foundCharacter.Name,
                                CurrentCountClans = 1,
                                MaxCountClans = 3
                            };

                            context.Alliances.Update(newAlliance);

                            await context.SaveChangesAsync(cancellationToken);

                            foundClanLeader.AllianceId = newAlliance.Id;

                            context.Clans.Update(foundClanLeader);

                            await context.SaveChangesAsync(cancellationToken);

                            foundCharacter.AllianceId = newAlliance.Id;

                            context.Characters.Update(foundCharacter);

                            await context.SaveChangesAsync(cancellationToken);

                            await _mediator.Send(new UpdateActivityCommand(request.UserId), cancellationToken);

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
            else
            {
                throw new AppException("You are not logged in.");
            }
        }
    }
}
