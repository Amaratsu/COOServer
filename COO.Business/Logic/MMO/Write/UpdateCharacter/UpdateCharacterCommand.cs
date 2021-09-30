using System.Collections.Generic;
using COO.Domain.Core;
using MediatR;

namespace COO.Business.Logic.MMO.Write.UpdateCharacter
{
    public sealed class UpdateCharacterCommand : IRequest<string>
    {
        public UpdateCharacterCommand(
            int userId, int characterId, int classId, int health, 
            int mana, int experience, int level, double posx, double posy, double posz, 
            double yaw, string equipChest, string equipFeet, string equipHands, 
            string equipHead, string equipLegs, string hotbar0, 
            string hotbar1, string hotbar2, string hotbar3, List<InventoryItem> inventory,
            List<Quest> quests, bool isOnline
        )
        {
            UserId = userId;
            CharacterId = characterId;
            ClassId = classId;
            Health = health;
            Mana = mana;
            Experience = experience;
            Level = level;
            PosX = posx;
            PosY = posy;
            PosZ = posz;
            RotationYaw = yaw;
            EquipChest = equipChest;
            EquipFeet = equipFeet;
            EquipHands = equipHands;
            EquipHead = equipHead;
            EquipLegs = equipLegs;
            Hotbar0 = hotbar0;
            Hotbar1 = hotbar1;
            Hotbar2 = hotbar2;
            Hotbar3 = hotbar3;
            Inventory = inventory;
            Quests = quests;
            IsOnline = isOnline;
        }

        public int UserId { get; set; }
        public int CharacterId { get; }
        public int ClassId { get; }
        public int Level { get; }
        public int Health { get; }
        public int Mana { get; }
        public int Experience { get; }
        public double PosX { get; }
        public double PosY { get; }
        public double PosZ { get; }
        public double RotationYaw { get; }
        public string EquipChest { get; }
        public string EquipFeet { get; }
        public string EquipHands { get; }
        public string EquipHead { get; }
        public string EquipLegs { get; }
        public string Hotbar0 { get; }
        public string Hotbar1 { get; }
        public string Hotbar2 { get; }
        public string Hotbar3 { get; }
        public List<InventoryItem> Inventory { get; }
        public List<Quest> Quests { get; }
        public bool IsOnline { get; set; }
    }
}