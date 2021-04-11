namespace COO.Domain.Core
{
    public class Clan
    {
        public int Id { get; set; }
        public int LeaderId { get; set; }
        public int? AllianceId { get; set; }
        public string LeaderName { get; set; }
        public string Name { get; set; }
        public int CurrentCountCharacters { get; set; }
        public int MaxCountCharacters { get; set; }
    }
}
