using System.Threading;
using System.Threading.Tasks;
using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Write.AddCharacterToClan
{
    public class AddCharacterToClanCommandHandler : IRequestHandler<AddCharacterToClanCommand, string>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;

        public AddCharacterToClanCommandHandler(IDbContextFactory<CooDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<string> Handle(AddCharacterToClanCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            //var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            //if (user != null)
            //{
            //    var character = user
            //        .Characters
            //        .Find(c => c.Name.ToLower() == request.CharacterName.ToLower());

            //    if (character != null)
            //    {
            //        if (character.ClanId != null)
            //        {
            //            throw new AppException("The character is already in a clan.");
            //        }
            //        else
            //        {
            //            var clan = await context.Clans.FirstOrDefaultAsync(c => c.Id == request.ClanId, cancellationToken);

            //            if (clan != null)
            //            {
            //                if (clan.CurrentCountCharacters + 1 > clan.MaxCountCharacters)
            //                {
            //                    throw new AppException("The clan has a maximum number of players.");
            //                }
            //                else
            //                {
            //                    character.ClanId = request.ClanId;

            //                    context.Users.Update(user);

            //                    clan.CurrentCountCharacters++;

            //                    context.Clans.Update(clan);

            //                    await context.SaveChangesAsync(cancellationToken);

            //                    return "OK";
            //                }
            //            }
            //            else
            //            {
            //                throw new AppException("The clan not found.");
            //            }
            //        }
            //    }
            //    else
            //    {
            //        throw new AppException("The character not found.");
            //    }
            //}
            //else
            //{
            //    throw new AppException("You are not logged in.");
            //}
            return "TODO";        
        }
    }
}
