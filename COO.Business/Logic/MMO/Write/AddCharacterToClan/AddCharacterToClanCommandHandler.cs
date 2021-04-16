﻿using System.Threading;
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

            var foundCharacter = await context
                .Characters
                .FirstOrDefaultAsync(c => c.Name.ToLower() == request.CharacterName.ToLower(), cancellationToken);

            if (foundCharacter != null)
            {
                if (foundCharacter.ClanId != null)
                {
                    throw new AppException("The character is already in a clan.");
                }
                else
                {
                    var foundClan =
                        await context.Clans.FirstOrDefaultAsync(c => c.Id == request.ClanId, cancellationToken);

                    if (foundClan != null)
                    {
                        if (foundClan.CurrentCountCharacters + 1 > foundClan.MaxCountCharacters)
                        {
                            throw new AppException("The clan has a maximum number of players.");
                        }
                        else
                        {
                            foundCharacter.ClanId = request.ClanId;

                            context.Characters.Update(foundCharacter);

                            foundClan.CurrentCountCharacters++;

                            context.Clans.Update(foundClan);

                            await context.SaveChangesAsync(cancellationToken);

                            return "OK";
                        }
                    }
                    else
                    {
                        throw new AppException("The clan not found.");
                    }
                }
            }
            else
            {
                throw new AppException("The character not found.");
            }
        }
    }
}
