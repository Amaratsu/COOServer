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
        private readonly IDbContextFactory<COODbContext> _contextFactory;

        public CreateCharacterCommandHandler(IDbContextFactory<COODbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<string> Handle(CreateCharacterCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var foundUser = await context.Users.FirstOrDefaultAsync(user => user.Id == request.UserId && user.Token == request.Token);

            if (foundUser != null)
            {
                if (foundUser.Name.ToLower() == request.Name.ToLower())
                {
                    throw new AppException("This name is unavailable");
                }
                else
                {
                    var fountInitializeDataCharacter = await context.InitializeDataCharacters
                        .FirstOrDefaultAsync(character => character.RaceId == request.RaceId && character.ClassId == request.ClassId);

                    if (fountInitializeDataCharacter != null)
                    {
                        var character = new Character
                        {
                            UserId = request.UserId,
                            Name = request.Name,
                            Gender = request.Gender,
                            Level = 1,
                            Experience = 0,
                            RaceId = fountInitializeDataCharacter.RaceId,
                            ClassId = fountInitializeDataCharacter.ClassId,
                            Health = fountInitializeDataCharacter.Health,
                            Mana = fountInitializeDataCharacter.Mana,
                            PosX = fountInitializeDataCharacter.PosX,
                            PosY = fountInitializeDataCharacter.PosY,
                            PosZ = fountInitializeDataCharacter.PosZ,
                            RotationYaw = fountInitializeDataCharacter.RotationYaw,
                            EquipChest = fountInitializeDataCharacter.EquipChest,
                            EquipFeet = fountInitializeDataCharacter.EquipFeet,
                            EquipHands = fountInitializeDataCharacter.EquipHands,
                            EquipHead = fountInitializeDataCharacter.EquipHead,
                            EquipLegs = fountInitializeDataCharacter.EquipLegs,
                            Hotbar0 = fountInitializeDataCharacter.Hotbar0,
                            Hotbar1 = fountInitializeDataCharacter.Hotbar1,
                            Hotbar2 = fountInitializeDataCharacter.Hotbar2,
                            Hotbar3 = fountInitializeDataCharacter.Hotbar3
                        };

                        await context.Characters.AddAsync(character, cancellationToken);

                        await context.SaveChangesAsync();

                        return "OK";
                    }
                    else
                    {
                        throw new AppException("The character could not be initialized.");
                    }
                }
            }
            else
            {
                throw new AppException("You are not logged in.");
            }
        }
    }
}
