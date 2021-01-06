namespace COO.Server.Features.Servers.Models
{
    public class AddServerRequestModel
    {
        public string ServerType { get; set; }
        public string IP { get; set; }
        public string Port { get; set; }
    }
}
