using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Write.GetCharacters
{
    public class GetCharactersCommandHadler : IRequestHandler<GetCharactersCommand, GetCharactersResponseModel>
    {
        private readonly IDbContextFactory<COODbContext> _contextFactory;

        public GetCharactersCommandHadler(IDbContextFactory<COODbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<GetCharactersResponseModel> Handle(GetCharactersCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var foundUser = await context.Users.FirstOrDefaultAsync(user => user.Id == request.UserId && user.Token == request.Token, cancellationToken);

            if (foundUser != null)
            {
                var response = new GetCharactersResponseModel { Characters = new List<CharacterModel>() };

                var foundCharacters = await context.Characters
                    .Where(c => c.Id == foundUser.Id && c.ServerId == request.ServerId)
                    .ToListAsync(cancellationToken);

                if (foundCharacters.Count > 0)
                {
                    foundCharacters.ForEach(c =>
                    {
                        var character = new CharacterModel
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
                        };
                        response.Characters.Add(character);
                    });
                }

                foundUser.LastActivity = DateTime.UtcNow;

                context.Users.Update(foundUser);

                await context.SaveChangesAsync(cancellationToken);

                return response;

            }
            else
            {
                throw new AppException("You are not logged in.");
            }
        }
    }
}
