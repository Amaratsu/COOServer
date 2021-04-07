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
        private readonly IDbContextFactory<COODbContext> _contextFactory;

        public DeleteCharacterCommandHandler(IDbContextFactory<COODbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<string> Handle(DeleteCharacterCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var foundUser = await context.Users.FirstOrDefaultAsync(user => user.Id == request.UserId && user.Token == request.Token, cancellationToken);

            if (foundUser != null)
            {
                var foundCharacter = await context.Characters
                        .FirstOrDefaultAsync(character => character.Id == request.CharacterId, cancellationToken);

                if (foundCharacter != null)
                {
                    context.Characters.Remove(foundCharacter);

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
