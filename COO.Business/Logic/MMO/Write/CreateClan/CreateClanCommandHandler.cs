using System.Threading;
using System.Threading.Tasks;
using COO.DataAccess.Contexts;
using COO.Domain.Core;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Write.CreateClan
{
    public class CreateClanCommandHandler : IRequestHandler<CreateClanCommand, string>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;

        public CreateClanCommandHandler(IDbContextFactory<CooDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<string> Handle(CreateClanCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var foundCharacter = await context
                .Characters
                .FirstOrDefaultAsync(c => c.Id == request.CharacterId, cancellationToken);

            if (foundCharacter != null)
            {

                var clan = await context
                    .Clans
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == request.ClanName.ToLower(), cancellationToken);

                if (clan != null)
                {
                    throw new AppException("This clan name is unavailable.");
                }
                else
                {
                    if (foundCharacter.ClanId != null)
                    {
                        throw new AppException("The character already has a clan");
                    }
                    else
                    {
                        var newClan = new Clan
                        {
                            LeaderId = foundCharacter.Id,
                            LeaderName = foundCharacter.Name,
                            Name = request.ClanName,
                            CurrentCountCharacters = 1,
                            MaxCountCharacters = 10
                        };

                        await context.Clans.AddAsync(newClan, cancellationToken);

                        await context.SaveChangesAsync(cancellationToken);

                        foundCharacter.ClanId = newClan.Id;

                        context.Characters.Update(foundCharacter);

                        await context.SaveChangesAsync(cancellationToken);

                        return "OK";
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
