using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Write.GetCharacter
{
    public class GetCharacterCommandHandler : IRequestHandler<GetCharacterCommand, GetCharacterResponseModel>
    {
        private readonly IDbContextFactory<COODbContext> _contextFactory;

        public GetCharacterCommandHandler(IDbContextFactory<COODbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<GetCharacterResponseModel> Handle(GetCharacterCommand request,
            CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var foundUser =
                await context.Users.
                    FirstOrDefaultAsync(u => u.Id == request.UserId && u.Token == request.Token, cancellationToken);

            if (foundUser != null)
            {
                var foundCharacter = await context.Characters
                    .FirstOrDefaultAsync(c => c.Id == request.CharacterId && c.ServerId == request.ServerId, cancellationToken);

                if (foundCharacter != null)
                {
                    var inventories = await context.Inventories
                        .Where(i => i.CharacterId == foundCharacter.Id)
                        .ToListAsync(cancellationToken);

                    var quests = await context.Quests
                        .Where(q => q.CharacterId == foundCharacter.Id)
                        .ToListAsync(cancellationToken);

                    var clan = await context.Clans
                        .FirstOrDefaultAsync(c => c.Id == foundCharacter.ClanId, cancellationToken);

                    foundUser.LastActivity = DateTime.UtcNow;

                    context.Users.Update(foundUser);

                    await context.SaveChangesAsync(cancellationToken);

                    return new GetCharacterResponseModel
                    {
                        CharacterId = foundCharacter.Id,
                        Name = foundCharacter.Name,
                        Gender = foundCharacter.Gender,
                        RaceId = foundCharacter.RaceId,
                        ClassId = foundCharacter.ClassId,
                        Health = foundCharacter.Health,
                        Mana = foundCharacter.Mana,
                        Level = foundCharacter.Level,
                        Experience = foundCharacter.Experience,
                        EquipChest = foundCharacter.EquipChest,
                        EquipFeet = foundCharacter.EquipFeet,
                        EquipHands = foundCharacter.EquipHands,
                        EquipHead = foundCharacter.EquipHead,
                        EquipLegs = foundCharacter.EquipLegs,
                        PosX = foundCharacter.PosX,
                        PosY = foundCharacter.PosY,
                        PosZ = foundCharacter.PosZ,
                        RotationYaw = foundCharacter.RotationYaw,
                        Hotbar0 = foundCharacter.Hotbar0,
                        Hotbar1 = foundCharacter.Hotbar1,
                        Hotbar2 = foundCharacter.Hotbar2,
                        Hotbar3 = foundCharacter.Hotbar3,
                        ClanName = clan.Name,
                        Inventory = inventories,
                        Quests = quests
                    };
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
