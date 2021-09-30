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

            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user != null)
            {
                var character = user.Characters.Find(character => character.Id == request.CharacterId);

                if (character != null)
                {
                        var clan = await context.Clans.FirstOrDefaultAsync(c => c.Id == request.ClanId, cancellationToken);

                        var foundCharacter = clan.Characters.Find(c => c.Id == character.Id);

                        if (foundCharacter == null)
                    {
                        throw new AppException("The Character is not in a clan.");
                    }
                    else
                    {
                        var response = new GetClanCharactersResponseModel
                        {
                            ClanCharacters = new List<ClanCharacterModel>()
                        };

                        if (clan.Characters.Count > 0)
                        {
                                clan.Characters.ForEach(fcc =>
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
            else
            {
                throw new AppException("You are not logged in.");
            }
        }
    }
}
