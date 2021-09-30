using System.Collections.Generic;
using COO.Domain.Core;

namespace COO.Business.Logic.MMO.Read.GetCharacter
{
    public class GetCharacterResponseModel
    {
        public int CharacterId { get; set; }
        public string Name { get; set; }
        public int Gender { get; set; }
        public int RaceId { get; set; }
        public int ClassId { get; set; }
        public List<InventoryItem> InventoryItems { get; set; }
        public double Health { get; set; }
        public double Mana { get; set; }
        public int Level { get; set; }
        public double Experience { get; set; }
        public string ClanName { get; set; }
        public string AllianceName { get; set; }
        public List<InventoryItem> Inventory { get; set; }
        public List<Quest> Quests { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double PosZ { get; set; }
        public InventoryItem Primary { get; set; }
        public InventoryItem Secondary { get; set; }
        public InventoryItem Helmet { get; set; }
        public InventoryItem BodyArmor { get; set; }
        public InventoryItem Gloves { get; set; }
        public InventoryItem Boots { get; set; }
        public InventoryItem Shoulder { get; set; }
        public InventoryItem Cape { get; set; }
        public InventoryItem Belt { get; set; }
        public InventoryItem Pants { get; set; }
        public InventoryItem Necklace { get; set; }
        public InventoryItem Ring1 { get; set; }
        public InventoryItem Ring2 { get; set; }
        public InventoryItem Earring1 { get; set; }
        public InventoryItem Earring2 { get; set; }
    }
}
