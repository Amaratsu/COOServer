namespace COO.Server.Data.Models
{
    public class Server
    {
        public int Id { get; set; }
        public string ServerType { get; set; }
        public string IP { get; set; }
        public string Port { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Region { get; set; }
        public bool IsInGame { get; set; }
        public int CNP { get; set; }
        public int MNP { get; set; }
        public string PG { get; set; }
        public string IG { get; set; }
    }
}
