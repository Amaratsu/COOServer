namespace COO.Business.Logic.MMO.Write.GetGameServers
{
    public class GameServer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IP { get; set; }
        public int CurrentCount { get; set; }
        public int MaxCount { get; set; }
        public int Status { get; set; }
    }
}
