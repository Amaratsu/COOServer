using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Write.DeleteCharacter
{
    public class DeleteCharacterCommandHandler : IRequestHandler<DeleteCharacterCommand, string>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;

        public DeleteCharacterCommandHandler(IDbContextFactory<CooDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<string> Handle(DeleteCharacterCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var character = await context.Characters
                    .FirstOrDefaultAsync(c => c.Id == request.CharacterId, cancellationToken);

            if (character != null)
            {
                var allianceLeaderInAlliance = await context.Alliances.FirstOrDefaultAsync(a => a.LeaderId == character.Id, cancellationToken);
                if (allianceLeaderInAlliance != null)
                {
                    var clans = await context.Clans.Where(c => c.AllianceId == allianceLeaderInAlliance.Id)
                        .ToListAsync(cancellationToken);
                    if (clans.Count > 0)
                    {
                        clans.ForEach(fc => fc.AllianceId = null);
                        context.Clans.UpdateRange(clans);
                    }
                    context.Alliances.Remove(allianceLeaderInAlliance);
                }

                var clanLeaderInClan = await context.Clans.FirstOrDefaultAsync(c => c.LeaderId == character.Id, cancellationToken);
                if (clanLeaderInClan != null)
                {
                    var clanCharacters = await context.Characters.Where(c => c.ClanId == clanLeaderInClan.Id)
                        .ToListAsync(cancellationToken);
                    if (clanCharacters.Count > 0)
                    {
                        clanCharacters.ForEach(fcc => fcc.ClanId = null);
                        context.Characters.UpdateRange(clanCharacters);
                    }
                    context.Clans.Remove(clanLeaderInClan);
                }

                var clanMemberInClan = await context.Clans.FirstOrDefaultAsync(c => c.Id == character.ClanId, cancellationToken);
                if (clanMemberInClan != null)
                {
                    clanMemberInClan.CurrentCountCharacters--;
                    context.Clans.Update(clanMemberInClan);
                }
                
                context.Characters.Remove(character);

                await context.SaveChangesAsync(cancellationToken);

                return "OK";
            }
            else
            {
                throw new AppException("The character not found.");
            }
        }
    }
}
