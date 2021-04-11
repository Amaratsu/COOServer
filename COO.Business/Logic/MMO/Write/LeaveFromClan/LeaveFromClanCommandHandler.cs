using System.Threading;
using System.Threading.Tasks;
using COO.Business.Logic.Account.Write.UpdateActivity;
using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Write.LeaveFromClan
{
    public class LeaveFromClanCommandHandler : IRequestHandler<LeaveFromClanCommand, string>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;
        private readonly IMediator _mediator;

        public LeaveFromClanCommandHandler(IDbContextFactory<CooDbContext> contextFactory, IMediator mediator)
        {
            _contextFactory = contextFactory;
            _mediator = mediator;
        }

        public async Task<string> Handle(LeaveFromClanCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var foundUser =
                await context.Users.FirstOrDefaultAsync(user => user.Id == request.UserId, cancellationToken);

            if (foundUser != null)
            {
                var foundCharacter = await context.Characters
                    .FirstOrDefaultAsync(character => character.Id == request.CharacterId, cancellationToken);

                var foundClan = await context.Clans.FirstOrDefaultAsync(c => c.LeaderId == foundCharacter.Id, cancellationToken);

                if (foundCharacter != null)
                {

                    if (foundClan != null) {

                        if (foundCharacter.ClanId != null)
                        {
                            foundCharacter.ClanId = null;

                            context.Characters.Update(foundCharacter);

                            foundClan.CurrentCountCharacters--;

                            context.Clans.Update(foundClan);

                            await context.SaveChangesAsync(cancellationToken);

                            await _mediator.Send(new UpdateActivityCommand(request.UserId), cancellationToken);

                            return "OK";
                        }
                        else
                        {
                            throw new AppException("The character is not in a clan.");
                        }
                    }
                    else
                    {
                        throw new AppException("Clan leader cannot leave the clan.");
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
