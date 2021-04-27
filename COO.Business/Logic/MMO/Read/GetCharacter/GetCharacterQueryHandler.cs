using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Read.GetCharacter
{
    public class GetCharacterQueryHandler : IRequestHandler<GetCharacterQuery, GetCharacterResponseModel>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;

        public GetCharacterQueryHandler(IDbContextFactory<CooDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<GetCharacterResponseModel> Handle(GetCharacterQuery request,
            CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var character = await context.Characters
                .FirstOrDefaultAsync(c => c.Id == request.CharacterId && c.GameServerId == request.ServerId, cancellationToken);

            if (character != null)
            {
                var inventories = await context.InventoryItems
                    .Where(i => i.CharacterId == character.Id)
                    .ToListAsync(cancellationToken);

                var quests = await context.Quests
                    .Where(q => q.CharacterId == character.Id)
                    .ToListAsync(cancellationToken);

                var clan = await context.Clans
                    .FirstOrDefaultAsync(c => c.Id == character.ClanId, cancellationToken);

                var allianceName = "";

                if (clan?.AllianceId != null)
                {
                    var alliance = await context.Alliances.FirstOrDefaultAsync(a => a.Id == clan.AllianceId, cancellationToken);
                    allianceName = alliance.Name;
                }

                return new GetCharacterResponseModel
                {
                    CharacterId = character.Id,
                    Name = character.Name,
                    Gender = character.Gender,
                    RaceId = character.RaceId,
                    ClassId = character.ClassId,
                    Health = character.Health,
                    Mana = character.Mana,
                    Level = character.Level,
                    Experience = character.Experience,
                    EquipChest = character.EquipChest,
                    EquipFeet = character.EquipFeet,
                    EquipHands = character.EquipHands,
                    EquipHead = character.EquipHead,
                    EquipLegs = character.EquipLegs,
                    PosX = character.PosX,
                    PosY = character.PosY,
                    PosZ = character.PosZ,
                    RotationYaw = character.RotationYaw,
                    Hotbar0 = character.Hotbar0,
                    Hotbar1 = character.Hotbar1,
                    Hotbar2 = character.Hotbar2,
                    Hotbar3 = character.Hotbar3,
                    ClanName = clan?.Name,
                    AllianceName = allianceName,
                    Inventory = inventories,
                    Quests = quests
                };
            }
            else
            {
                throw new AppException("The character not found.");
            }
        }
    }
}
