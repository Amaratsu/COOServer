namespace COO.Business.Logic.MMO.Read.GetClanCharacters
{
    public class ClanCharacterModel
    {
        public string Name { get; set; }
        public bool IsLeader { get; set; }
        public int Level { get; set; }
        public int RaceId { get; set; }
        public int ClassId { get; set; }
        public bool IsOnline { get; set; }
    }
}
