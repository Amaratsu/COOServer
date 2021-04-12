using System.Threading;
using System.Threading.Tasks;
using COO.Business.Logic.Account.Write.UpdateActivity;
using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Write.DeleteClanFromAlliance
{
    public class DeleteClanFromAllianceCommandHandler : IRequestHandler<DeleteClanFromAllianceCommand, string>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;
        private readonly IMediator _mediator;

        public DeleteClanFromAllianceCommandHandler(IDbContextFactory<CooDbContext> contextFactory, IMediator mediator)
        {
            _contextFactory = contextFactory;
            _mediator = mediator;
        }

        public async Task<string> Handle(DeleteClanFromAllianceCommand request, CancellationToken cancellationToken)
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
                    if (allianceLeaderInAlliance != null)
                    {
                        var deleteClanFromAlliance = await context
                            .Clans
                            .FirstOrDefaultAsync(c => c.Name.ToLower() == request.ClanName.ToLower(), cancellationToken);

                        if (deleteClanFromAlliance != null)
                        {
                            if (deleteClanFromAlliance.LeaderId == foundCharacter.Id)
                            {
                                deleteClanFromAlliance.AllianceId = null;
                                context.Clans.Update(deleteClanFromAlliance);

                                allianceLeaderInAlliance.CurrentCountClans--;
                                context.Alliances.Update(allianceLeaderInAlliance);

                                await context.SaveChangesAsync(cancellationToken);

                                await _mediator.Send(new UpdateActivityCommand(request.UserId), cancellationToken);
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
            else
            {
                throw new AppException("You are not logged in.");
            }
        }
    }
}
