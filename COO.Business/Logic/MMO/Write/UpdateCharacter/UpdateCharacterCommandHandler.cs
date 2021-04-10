using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using COO.Business.Logic.Account.Write.UpdateActivity;
using COO.DataAccess.Contexts;
using COO.Domain.Core;
using COO.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace COO.Business.Logic.MMO.Write.UpdateCharacter
{
    public class UpdateCharacterCommandHandler : IRequestHandler<UpdateCharacterCommand, string>
    {
        private readonly IDbContextFactory<CooDbContext> _contextFactory;
        private readonly IMediator _mediator;


        public UpdateCharacterCommandHandler(IDbContextFactory<CooDbContext> contextFactory, IMediator mediator)
        {
            _contextFactory = contextFactory;
            _mediator = mediator;
        }

        public async Task<string> Handle(UpdateCharacterCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var foundUser = await context
                .Users
                .FirstOrDefaultAsync(user => user.Id == request.UserId, cancellationToken);
            
            if (foundUser != null)
            {
                var foundCharacter = await context
                    .Characters
                    .FirstOrDefaultAsync(c => c.Id == request.CharacterId, cancellationToken);

                if (foundCharacter != null)
                {
                    foundCharacter.ClassId = request.ClassId;
                    foundCharacter.Health = request.Health;
                    foundCharacter.Mana = request.Mana;
                    foundCharacter.Experience = request.Experience;
                    foundCharacter.Level = request.Level;
                    foundCharacter.PosX = request.PosX;
                    foundCharacter.PosY = request.PosY;
                    foundCharacter.PosZ = request.PosZ;
                    foundCharacter.RotationYaw = request.RotationYaw;
                    foundCharacter.EquipChest = request.EquipChest;
                    foundCharacter.EquipFeet = request.EquipFeet;
                    foundCharacter.EquipHands = request.EquipHands;
                    foundCharacter.EquipHead = request.EquipHead;
                    foundCharacter.EquipLegs = request.EquipLegs;
                    foundCharacter.Hotbar0 = request.Hotbar0;
                    foundCharacter.Hotbar1 = request.Hotbar1;
                    foundCharacter.Hotbar2 = request.Hotbar2;
                    foundCharacter.Hotbar3 = request.Hotbar3;
                    foundCharacter.IsOnline = request.IsOnline;
                    
                    await context.Characters.AddAsync(foundCharacter, cancellationToken);

                    if (request.Inventory != null && request.Inventory.Count > 0)
                    {
                        var oldInventory = await context
                            .InventoryItems
                            .Where(i => i.CharacterId == request.CharacterId)
                            .ToListAsync(cancellationToken);

                        if (oldInventory.Count > 0)
                        {
                            context.InventoryItems.RemoveRange(oldInventory);
                        }
                        
                        var oldQuests = await context
                            .Quests
                            .Where(i => i.CharacterId == request.CharacterId)
                            .ToListAsync(cancellationToken);

                        if (oldQuests.Count > 0)
                        {
                            context.Quests.RemoveRange(oldQuests);
                        }

                        if (request.Inventory.Count > 0)
                        {
                            var inventory = new List<InventoryItem>();
                            request.Inventory.ForEach(i => {
                                inventory.Add(new InventoryItem {
                                    CharacterId = request.CharacterId,
                                    Slot = i.Slot,
                                    Item = i.Item,
                                    Amount = i.Amount
                                });
                            });
                            
                            if (inventory.Count > 0)
                            {
                                await context.InventoryItems.AddRangeAsync(inventory, cancellationToken);
                            }
                        }
                        
                        if (request.Quests.Count > 0)
                        {
                            var quests = new List<Quest>();
                            request.Quests.ForEach(q => {
                                if (q.Completed)
                                {
                                    quests.Add(new Quest
                                    {
                                        CharacterId = request.CharacterId,
                                        Name = q.Name,
                                        Completed = q.Completed,
                                        Task1 = 0,
                                        Task2 = 0,
                                        Task3 = 0,
                                        Task4 = 0
                                    });
                                }
                                else
                                {
                                    quests.Add(new Quest
                                    {
                                        CharacterId = request.CharacterId,
                                        Name = q.Name,
                                        Completed = q.Completed,
                                        Task1 = q.Task1,
                                        Task2 = q.Task2,
                                        Task3 = q.Task3,
                                        Task4 = q.Task4
                                    });
                                }
                        
                            });

                            if (quests.Count > 0)
                            {
                                await context.Quests.AddRangeAsync(quests, cancellationToken);
                            }
                        }
                    }
                    await context.SaveChangesAsync(cancellationToken);

                    await _mediator.Send(new UpdateActivityCommand(request.UserId), cancellationToken);
                    
                    return "OK";
                }
                throw new AppException("The character not found.");
            }
            else
            {
                throw new AppException("You are not logged in.");
            }
        }
    }
}