using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using COO.Business.Logic.Account.Write.UpdateActivity;
using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Read.GetClanCharacters
{
    public class GetClanCharactersCommandHandler : IRequestHandler<GetClanCharactersCommand, GetClanCharactersResponseModel>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;
        private readonly IMediator _mediator;

        public GetClanCharactersCommandHandler(IDbContextFactory<CooDbContext> contextFactory, IMediator mediator)
        {
            _contextFactory = contextFactory;
            _mediator = mediator;
        }

        public async Task<GetClanCharactersResponseModel> Handle(GetClanCharactersCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var foundUser =
                await context.Users.FirstOrDefaultAsync(user => user.Id == request.UserId, cancellationToken);

            if (foundUser != null)
            {
                var foundCharacter = await context.Characters
                    .FirstOrDefaultAsync(character => character.Id == request.CharacterId, cancellationToken);

                if (foundCharacter != null)
                {
                    if (foundCharacter.ClanId == null)
                    {
                        throw new AppException("The Character is not in a clan.");
                    }
                    else
                    {
                        var foundClanCharacters = await context.Characters.Where(c => c.ClanId == foundCharacter.ClanId)
                            .ToListAsync(cancellationToken);

                        var clan = await context.Clans.FirstOrDefaultAsync(c => c.LeaderId == foundCharacter.Id,
                            cancellationToken);

                        var response = new GetClanCharactersResponseModel
                        {
                            ClanCharacters = new List<ClanCharacterModel>()
                        };

                        if (foundClanCharacters.Count > 0)
                        {
                            foundClanCharacters.ForEach(fcc =>
                            {
                                var clanCharacter = new ClanCharacterModel
                                {
                                    Name = fcc.Name,
                                    IsOnline = fcc.IsOnline,
                                    IsLeader = clan != null ? true : false,
                                    ClassId = fcc.ClassId,
                                    Level = fcc.Level,
                                    RaceId = fcc.RaceId
                                };
                                response.ClanCharacters.Add(clanCharacter);
                            });
                        }

                        await _mediator.Send(new UpdateActivityCommand(request.UserId), cancellationToken);

                        return response;
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
