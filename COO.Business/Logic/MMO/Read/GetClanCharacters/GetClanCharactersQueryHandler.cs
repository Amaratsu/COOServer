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

            var character = await context.Characters
                .FirstOrDefaultAsync(character => character.Id == request.CharacterId, cancellationToken);

            if (character != null)
            {
                if (character.ClanId == null)
                {
                    throw new AppException("The Character is not in a clan.");
                }
                else
                {
                    var clanCharacters = await context.Characters.Where(c => c.ClanId == character.ClanId)
                        .ToListAsync(cancellationToken);

                    var clan = await context.Clans.FirstOrDefaultAsync(c => c.LeaderId == character.Id,
                        cancellationToken);

                    var response = new GetClanCharactersResponseModel
                    {
                        ClanCharacters = new List<ClanCharacterModel>()
                    };

                    if (clanCharacters.Count > 0)
                    {
                        clanCharacters.ForEach(fcc =>
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
