using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Read.GetCharacters
{
    public class GetCharactersQueryHandler : IRequestHandler<GetCharactersQuery, GetCharactersResponseModel>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;

        public GetCharactersQueryHandler(IDbContextFactory<CooDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<GetCharactersResponseModel> Handle(GetCharactersQuery request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user != null)
            {
                var response = new GetCharactersResponseModel { Characters = new List<CharacterModel>() };

                var characters = await context.Characters
                    .Where(c => c.UserId == user.Id && c.GameServerId == request.ServerId)
                    .ToListAsync(cancellationToken);

                if (characters.Count > 0)
                {
                    characters.ForEach(c =>
                    {
                        response.Characters.Add(new CharacterModel
                        {
                            CharacterId = c.Id,
                            Name = c.Name,
                            Gender = c.Gender,
                            RaceId = c.RaceId,
                            ClassId = c.ClassId,
                            Level = c.Level,
                            Experience = c.Experience,
                            Health = c.Health,
                            Mana = c.Mana,
                            EquipChest = c.EquipChest,
                            EquipFeet = c.EquipFeet,
                            EquipHands = c.EquipHands,
                            EquipHead = c.EquipHead,
                            EquipLegs = c.EquipLegs
                        });
                    });
                }

                return response;
            }
            else
            {
                throw new AppException("You are not logged in.");
            }
        }
    }
}
