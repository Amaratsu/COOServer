namespace COO.Business.Logic.MMO.Read.GetClans
{
    public class ClanModel
    {
        public string Name { get; set; }
        public string LeaderName { get; set; }
        public int OnlineCount { get; set; }
        public int CurrentCountCharacters { get; set; }
        public int MaxCountCharacters { get; set; }
    }
}
