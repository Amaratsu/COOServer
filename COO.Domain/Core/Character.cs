using System.Collections.Generic;

namespace COO.Domain.Core
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Gender { get; set; }
        public int RaceId { get; set; }
        public int ClassId { get; set; }
        // bag
        public List<InventoryItem> InventoryItems { get; set; }
        // equipment items
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
        // position
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double PosZ { get; set; }
        // money
        public int Gold { get; set; }
        public int Silver { get; set; }
        public int Copper { get; set; }
        // action bars
        public string Actionbar1 { get; set; }
        public string Actionbar2 { get; set; }
        public string Actionbar3 { get; set; }
        public string Actionbar4 { get; set; }
        public string Actionbar5 { get; set; }
        public string Actionbar6 { get; set; }
        public int AllocationStats { get; set; }
        public List<string> ActiveSkills { get; set; }
        public int SkillPoints { get; set; }
        // stats
        public int Level { get; set; }
        public double Experience { get; set; }
        public double RequiredExperience { get; set; }
        public double Health { get; set; }
        public double MaxHealth { get; set; }
        public double Mana { get; set; }
        public double MaxMana { get; set; }
        public double Str { get; set; }
        public double Dex { get; set; }
        public double Int { get; set; }
        public double AttackX { get; set; }
        public double AttackY { get; set; }
        public double AttackSpeed { get; set; }
        public double Accuracy { get; set; }
        public double HealthRegen { get; set; }
        public double ManaRegen { get; set; }
        public double Armor { get; set; }
        public double LifeSteal { get; set; }
        public double CritChance { get; set; }
        public double CritMultiplier { get; set; }
        public double FireResistance { get; set; }
        public double LightningResistance { get; set; }
        public double ColdResistance { get; set; }
        public double DarkResistance { get; set; }
        public double MagicResistance { get; set; }
        public double PoisonResistance { get; set; }
        public double BleedResistance { get; set; }
        public double MovementSpeed { get; set; }
        public double Endurance { get; set; }

        public StatsAllocatorTracker StatsAllocatorTracker { get; set; }

        public List<int> ListOfEffects { get; set; }
        public List<int> EffectTrackers { get; set; }

        public bool IsOnline { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int GameServerId { get; set; }
        public GameServer GameServer { get; set; }

        //public int? Groupid { get; set; }
        //public Group Group { get; set; }

        //public int? Allianceid { get; set; }
        //public Alliance Alliance { get; set; }
    }
}
