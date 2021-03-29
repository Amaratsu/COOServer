using System.Collections.Generic;

namespace COO.Server.Controllers.MMO.Models
{
    public class SaveCharacterRequestModel
    {
        public int CharacterId { get; set; }
        public List<Inventory> Inventory { get; set; }
        public List<Quest> Quests { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int Experience { get; set; }
        public int Level { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double PosZ { get; set; }
        public double RotationYaw { get; set; }
        public string EquipChest { get; set; }
        public string EquipFeet { get; set; }
        public string EquipHands { get; set; }
        public string EquipHead { get; set; }
        public string EquipLegs { get; set; }
        public string Hotbar0 { get; set; }
        public string Hotbar1 { get; set; }
        public string Hotbar2 { get; set; }
        public string Hotbar3 { get; set; }
    }
}
