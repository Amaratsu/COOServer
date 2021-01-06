namespace COO.Server.Features.Servers.Models
{
    public class SetServerPasswordRequestModel
    {
        public string IP { get; set; }
        public string Port { get; set; }
        public string Password { get; set; }
    }
}
