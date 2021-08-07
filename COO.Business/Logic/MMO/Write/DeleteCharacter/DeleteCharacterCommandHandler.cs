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

            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user != null)
            {

                var character = user.Characters.Find(c => c.Id == request.CharacterId);

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
                        context.Clans.Remove(clanLeaderInClan);
                    }

                    var clanMemberInClan = await context.Clans.FirstOrDefaultAsync(c => c.Characters.Exists(ch => ch.UserId == character.Id), cancellationToken);
                    if (clanMemberInClan != null)
                    {
                        var deleteCharacter = clanMemberInClan.Characters.Find(c => c.Id == request.CharacterId);
                        if (deleteCharacter != null)
                        {
                            clanMemberInClan.CurrentCountCharacters--;
                            clanMemberInClan.Characters.Remove(deleteCharacter);

                            context.Clans.Update(clanMemberInClan);
                        }
                    }

                    var server = await context.InfoServers.FirstOrDefaultAsync(infoServer => infoServer.Id == request.ServerId);
                    if (server != null)
                    {
                        var deleteServerCharacter = server.Characters.Find(ch => ch.Id == request.CharacterId);
                        if (deleteServerCharacter != null)
                        {
                            server.Characters.Remove(deleteServerCharacter);
                            context.InfoServers.Update(server);
                        }
                    }
                
                    context.Users.Update(user);

                    await context.SaveChangesAsync(cancellationToken);

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
