namespace COO.Server.Features.Servers.Models
{
    public class ServerResponseModel
    {
        public int Id { get; set; }
        public string IP { get; set; }
        public string Port { get; set; }
        public string MapName { get; set; }
        public int CNP { get; set; }
        public int MNP { get; set; }
    }
}
