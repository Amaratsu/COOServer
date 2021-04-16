using COO.DataAccess.Contexts;
using COO.Domain.Core;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace COO.Business.Logic.MMO.Write.CreateCharacter
{
    public class CreateCharacterCommandHandler : IRequestHandler<CreateCharacterCommand, string>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;

        public CreateCharacterCommandHandler(IDbContextFactory<CooDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<string> Handle(CreateCharacterCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var foundCharacter = await context.Characters
                .AnyAsync(c => c.Name == request.Name, cancellationToken);

            if (foundCharacter)
            {
                throw new AppException("This name is unavailable");
            }

            var foundInitializeDataCharacter = await context.InitializeDataCharacters
                .FirstOrDefaultAsync(character => character.RaceId == request.RaceId && character.ClassId == request.ClassId, cancellationToken);

            if (foundInitializeDataCharacter != null)
            {
                var character = new Character
                {
                    UserId = request.UserId,
                    ServerId = request.ServerId,
                    Name = request.Name,
                    Gender = request.Gender,
                    Level = 1,
                    Experience = 0,
                    RaceId = foundInitializeDataCharacter.RaceId,
                    ClassId = foundInitializeDataCharacter.ClassId,
                    Health = foundInitializeDataCharacter.Health,
                    Mana = foundInitializeDataCharacter.Mana,
                    PosX = foundInitializeDataCharacter.PosX,
                    PosY = foundInitializeDataCharacter.PosY,
                    PosZ = foundInitializeDataCharacter.PosZ,
                    RotationYaw = foundInitializeDataCharacter.RotationYaw,
                    EquipChest = foundInitializeDataCharacter.EquipChest,
                    EquipFeet = foundInitializeDataCharacter.EquipFeet,
                    EquipHands = foundInitializeDataCharacter.EquipHands,
                    EquipHead = foundInitializeDataCharacter.EquipHead,
                    EquipLegs = foundInitializeDataCharacter.EquipLegs,
                    Hotbar0 = foundInitializeDataCharacter.Hotbar0,
                    Hotbar1 = foundInitializeDataCharacter.Hotbar1,
                    Hotbar2 = foundInitializeDataCharacter.Hotbar2,
                    Hotbar3 = foundInitializeDataCharacter.Hotbar3
                };

                await context.Characters.AddAsync(character, cancellationToken);

                await context.SaveChangesAsync(cancellationToken);

                return "OK";
            }
            else
            {
                throw new AppException("The character could not be initialized.");
            }
        }
    }
}
