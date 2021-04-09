using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using COO.Business.Logic.Account.Write.UpdateActivity;
using COO.DataAccess.Contexts;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Read.GetCharacter
{
    public class GetCharacterQueryHandler : IRequestHandler<GetCharacterQuery, GetCharacterResponseModel>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;
        private readonly IMediator _mediator;

        public GetCharacterQueryHandler(IDbContextFactory<CooDbContext> contextFactory, IMediator mediator)
        {
            _contextFactory = contextFactory;
            _mediator = mediator;
        }

        public async Task<GetCharacterResponseModel> Handle(GetCharacterQuery request,
            CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var foundUser =
                await context.Users.
                    FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (foundUser != null)
            {
                var foundCharacter = await context.Characters
                    .FirstOrDefaultAsync(c => c.Id == request.CharacterId && c.ServerId == request.ServerId, cancellationToken);

                if (foundCharacter != null)
                {
                    var inventories = await context.InventoryItems
                        .Where(i => i.CharacterId == foundCharacter.Id)
                        .ToListAsync(cancellationToken);

                    var quests = await context.Quests
                        .Where(q => q.CharacterId == foundCharacter.Id)
                        .ToListAsync(cancellationToken);

                    var clan = await context.Clans
                        .FirstOrDefaultAsync(c => c.Id == foundCharacter.ClanId, cancellationToken);

                    await _mediator.Send(new UpdateActivityCommand(request.UserId), cancellationToken);

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
