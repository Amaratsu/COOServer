namespace COO.Server.Data.Models
{
    public class ClanCharacter
    {
        public int CharacterId { get; set; }
        public string CharacterName { get; set; }
        public int ClanId { get; set; }
        public string ClanName { get; set; }
        public bool IsLeader { get; set; }
    }
}
