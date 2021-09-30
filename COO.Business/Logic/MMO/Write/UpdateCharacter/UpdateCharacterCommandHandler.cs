using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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


        public UpdateCharacterCommandHandler(IDbContextFactory<CooDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<string> Handle(UpdateCharacterCommand request, CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateDbContext();

            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user != null)
            {

                var character = user.Characters.Find(c => c.Id == request.CharacterId);

                if (character != null)
                {
                    character.ClassId = request.ClassId;
                    character.Health = request.Health;
                    character.Mana = request.Mana;
                    character.Experience = request.Experience;
                    character.Level = request.Level;
                    character.PosX = request.PosX;
                    character.PosY = request.PosY;
                    character.PosZ = request.PosZ;
                    //character.RotationYaw = request.RotationYaw;
                    //character.EquipChest = request.EquipChest;
                    //character.EquipFeet = request.EquipFeet;
                    //character.EquipHands = request.EquipHands;
                    //character.EquipHead = request.EquipHead;
                    //character.EquipLegs = request.EquipLegs;
                    //character.Hotbar0 = request.Hotbar0;
                    //character.Hotbar1 = request.Hotbar1;
                    //character.Hotbar2 = request.Hotbar2;
                    //character.Hotbar3 = request.Hotbar3;
                    character.IsOnline = request.IsOnline;               

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
                                    //Slot = i.Slot,
                                    //Item = i.Item,
                                    //Amount = i.Amount
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

                    context.Users.Update(user);

                    await context.SaveChangesAsync(cancellationToken);

                    return "OK";
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