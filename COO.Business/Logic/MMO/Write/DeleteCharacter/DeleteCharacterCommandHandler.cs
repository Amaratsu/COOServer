using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using COO.Business.Logic.Account.Write.UpdateActivity;
using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Write.DeleteCharacter
{
    public class DeleteCharacterCommandHandler : IRequestHandler<DeleteCharacterCommand, string>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;
        private readonly IMediator _mediator;

        public DeleteCharacterCommandHandler(IDbContextFactory<CooDbContext> contextFactory, IMediator mediator)
        {
            _contextFactory = contextFactory;
            _mediator = mediator;
        }

        public async Task<string> Handle(DeleteCharacterCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var foundUser = await context.Users.FirstOrDefaultAsync(user => user.Id == request.UserId, cancellationToken);

            if (foundUser != null)
            {
                var foundCharacter = await context.Characters
                        .FirstOrDefaultAsync(character => character.Id == request.CharacterId, cancellationToken);

                if (foundCharacter != null)
                {
                    var allianceLeaderInAlliance = await context.Alliances.FirstOrDefaultAsync(a => a.LeaderId == foundCharacter.Id, cancellationToken);
                    if (allianceLeaderInAlliance != null)
                    {
                        var foundClans = await context.Clans.Where(c => c.AllianceId == allianceLeaderInAlliance.Id)
                            .ToListAsync(cancellationToken);
                        if (foundClans.Count > 0)
                        {
                            foundClans.ForEach(fc => fc.AllianceId = null);
                            context.Clans.UpdateRange(foundClans);
                        }
                        context.Alliances.Remove(allianceLeaderInAlliance);
                    }

                    var clanLeaderInClan = await context.Clans.FirstOrDefaultAsync(c => c.LeaderId == foundCharacter.Id, cancellationToken);
                    if (clanLeaderInClan != null)
                    {
                        var foundClanCharacters = await context.Characters.Where(c => c.ClanId == clanLeaderInClan.Id)
                            .ToListAsync(cancellationToken);
                        if (foundClanCharacters.Count > 0)
                        {
                            foundClanCharacters.ForEach(fcc => fcc.ClanId = null);
                            context.Characters.UpdateRange(foundClanCharacters);
                        }
                        context.Clans.Remove(clanLeaderInClan);
                    }

                    var clanMemberInClan = await context.Clans.FirstOrDefaultAsync(c => c.Id == foundCharacter.ClanId, cancellationToken);
                    if (clanMemberInClan != null)
                    {
                        clanMemberInClan.CurrentCountCharacters--;
                        context.Clans.Update(clanMemberInClan);
                    }
                    
                    context.Characters.Remove(foundCharacter);

                    await context.SaveChangesAsync(cancellationToken);

                    await _mediator.Send(new UpdateActivityCommand(request.UserId), cancellationToken);

                    return "OK";
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
