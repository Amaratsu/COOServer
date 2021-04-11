namespace COO.Business.Logic.MMO.Read.GetGameServers
{
    public class GameServerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IP { get; set; }
        public int CurrentCount { get; set; }
        public int MaxCount { get; set; }
        public int Status { get; set; }
    }
}
