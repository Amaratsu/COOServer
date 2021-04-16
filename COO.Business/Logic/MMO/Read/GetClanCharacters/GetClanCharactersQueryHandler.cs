using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Read.GetClanCharacters
{
    public class GetClanCharactersQueryHandler : IRequestHandler<GetClanCharactersQuery, GetClanCharactersResponseModel>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;

        public GetClanCharactersQueryHandler(IDbContextFactory<CooDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<GetClanCharactersResponseModel> Handle(GetClanCharactersQuery request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

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
                            response.ClanCharacters.Add(new ClanCharacterModel
                            {
                                Name = fcc.Name,
                                IsOnline = fcc.IsOnline,
                                IsLeader = clan != null,
                                ClassId = fcc.ClassId,
                                Level = fcc.Level,
                                RaceId = fcc.RaceId
                            });
                        });
                    }

                    return response;
                }
            }
            else
            {
                throw new AppException("The character not found.");
            }
        }
    }
}
