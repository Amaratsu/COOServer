﻿using System.Threading;
using System.Threading.Tasks;
using COO.Business.Logic.Account.Write.UpdateActivity;
using COO.Business.Logic.MMO.Write.DisbandClan;
using COO.Business.Logic.MMO.Write.LeaveFromClan;
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
                    var foundClanLeader = await context.Clans.FirstOrDefaultAsync(c => c.LeaderId == foundCharacter.Id, cancellationToken);

                    if (foundClanLeader != null)
                    {
                        await _mediator.Send(new DisbandClanCommand(foundUser.Id, foundCharacter.Id), cancellationToken);
                    }

                    var foundCharacterClan = await context.Clans.FirstOrDefaultAsync(c => c.Id == foundCharacter.ClanId, cancellationToken);

                    if (foundCharacterClan != null)
                    {
                        await _mediator.Send(new LeaveFromClanCommand(foundUser.Id, foundCharacter.Id), cancellationToken);
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
