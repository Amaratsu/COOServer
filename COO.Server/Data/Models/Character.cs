namespace COO.Server.Data.Models
{
    public class Character
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public int Gender { get; set; }
        public int Clan { get; set; }
        public int Class { get; set; }
        public int Level { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int Experience { get; set; }
        public int? PosX { get; set; }
        public int? PosY { get; set; }
        public int? PosZ { get; set; }
        public decimal RotationYaw { get; set; }
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
