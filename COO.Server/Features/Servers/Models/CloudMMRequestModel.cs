namespace COO.Server.Features.Servers.Models
{
    public class CloudMMRequestModel
    {
        public string Requester { get; set; }
        public string Members { get; set; }
        public string HostRequest { get; set; }
        public string GameType { get; set; }
        public int TeamCount { get; set; }
        public int MNP { get; set; }
        public bool IsCanceled { get; set; }
    }
}
