namespace COO.Business.Logic.MMO.Read.GetCharacters
{
    public class CharacterModel
    {
        public int CharacterId { get; set; }
        public string Name { get; set; }
        public int Gender { get; set; }
        public int RaceId { get; set; }
        public int ClassId { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public string EquipChest { get; set; }
        public string EquipFeet { get; set; }
        public string EquipHands { get; set; }
        public string EquipHead { get; set; }
        public string EquipLegs { get; set; }
    }
}
