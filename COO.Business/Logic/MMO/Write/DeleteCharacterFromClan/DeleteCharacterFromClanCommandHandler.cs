using System.Threading;
using System.Threading.Tasks;
using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Write.DeleteCharacterFromClan
{
    public class DeleteCharacterFromClanCommandHandler : IRequestHandler<DeleteCharacterFromClanCommand, string>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;

        public DeleteCharacterFromClanCommandHandler(IDbContextFactory<CooDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<string> Handle(DeleteCharacterFromClanCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user != null)
            {
                var character = user.Characters.Find(c => c.Id == request.CharacterId);

                if (character != null)
                {
                    var clan = await context.Clans.FirstOrDefaultAsync(c => c.LeaderId == character.Id, cancellationToken);

                    if (clan != null)
                    {
                        var deleteCharacterFromClan = clan
                            .Characters
                            .Find(c => c.Name.ToLower() == request.CharacterName.ToLower());

                        if (deleteCharacterFromClan != null)
                        {
                            clan.Characters.Remove(deleteCharacterFromClan);

                            clan.CurrentCountCharacters--;

                            await context.SaveChangesAsync(cancellationToken);

                            return "OK";
                        }
                        else
                        {
                            throw new AppException("The character is not in a clan.");
                        }
                    }
                    else
                    {
                        throw new AppException("Only the clan leader can remove a player.");
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
