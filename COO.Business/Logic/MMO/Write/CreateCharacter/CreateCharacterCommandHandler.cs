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

            var character = await context
                .Characters
                .AnyAsync(c => c.Name == request.Name, cancellationToken);

            if (character)
            {
                throw new AppException("This name is unavailable");
            }

            var initializeDataCharacter = await context
                .InitializeDataCharacters
                .FirstOrDefaultAsync(c => c.RaceId == request.RaceId && c.ClassId == request.ClassId, cancellationToken);

            if (initializeDataCharacter != null)
            {
                var newCharacter = new Character
                {
                    UserId = request.UserId,
                    GameServerId = request.ServerId,
                    Name = request.Name,
                    Gender = request.Gender,
                    Level = 1,
                    Experience = 0,
                    RaceId = initializeDataCharacter.RaceId,
                    ClassId = initializeDataCharacter.ClassId,
                    Health = initializeDataCharacter.Health,
                    Mana = initializeDataCharacter.Mana,
                    PosX = initializeDataCharacter.PosX,
                    PosY = initializeDataCharacter.PosY,
                    PosZ = initializeDataCharacter.PosZ,
                    RotationYaw = initializeDataCharacter.RotationYaw,
                    EquipChest = initializeDataCharacter.EquipChest,
                    EquipFeet = initializeDataCharacter.EquipFeet,
                    EquipHands = initializeDataCharacter.EquipHands,
                    EquipHead = initializeDataCharacter.EquipHead,
                    EquipLegs = initializeDataCharacter.EquipLegs,
                    Hotbar0 = initializeDataCharacter.Hotbar0,
                    Hotbar1 = initializeDataCharacter.Hotbar1,
                    Hotbar2 = initializeDataCharacter.Hotbar2,
                    Hotbar3 = initializeDataCharacter.Hotbar3
                };

                await context.Characters.AddAsync(newCharacter, cancellationToken);

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
