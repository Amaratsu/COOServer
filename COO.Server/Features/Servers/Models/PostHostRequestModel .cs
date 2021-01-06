namespace COO.Server.Features.Servers.Models
{
    public class PostHostRequestModel
    {
        public string Hosts { get; set; }
        public string ServerType { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Region { get; set; }
        public int MNP { get; set; }
        public string PG { get; set; }
        public string IG { get; set; }
    }
}
