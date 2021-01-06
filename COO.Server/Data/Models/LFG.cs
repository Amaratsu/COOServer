namespace COO.Server.Data.Models
{
    public class LFG
    {
        public int Id { get; set; }
        public string Requester { get; set; }
        public string Members { get; set; }
        public string HostRequest { get; set; }
        public string GameType { get; set; }
        public int TeamCount { get; set; }
        public int MNP { get; set; }
        public bool IsCanceled { get; set; }
    }
}
