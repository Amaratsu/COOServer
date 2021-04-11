﻿using System.Threading;
using System.Threading.Tasks;
using COO.Business.Logic.Account.Write.UpdateActivity;
using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Write.DeleteCharacterFromClan
{
    public class DeleteCharacterFromClanCommandHandler : IRequestHandler<DeleteCharacterFromClanCommand, string>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;
        private readonly IMediator _mediator;

        public DeleteCharacterFromClanCommandHandler(IDbContextFactory<CooDbContext> contextFactory, IMediator mediator)
        {
            _contextFactory = contextFactory;
            _mediator = mediator;
        }

        public async Task<string> Handle(DeleteCharacterFromClanCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var foundUser = await context.Users.FirstOrDefaultAsync(user => user.Id == request.UserId, cancellationToken);

            if (foundUser != null)
            {
                var foundCharacter = await context.Characters
                    .FirstOrDefaultAsync(character => character.Id == request.CharacterId, cancellationToken);

                if (foundCharacter != null)
                {
                    var foundClan = await context.Clans.FirstOrDefaultAsync(c => c.LeaderId == foundCharacter.Id, cancellationToken);

                    if (foundClan != null)
                    {
                        var deleteCharacterFromClan = await context
                            .Characters
                            .FirstOrDefaultAsync(c => c.ClanId == foundClan.Id && c.Name.ToLower() == request.CharacterName.ToLower(), cancellationToken);

                        if (deleteCharacterFromClan != null)
                        {
                            deleteCharacterFromClan.ClanId = null;

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
