namespace COO.Server.Features.Servers.Models
{
    public class SetMyServerIPRequestModel
    {
        public string Username { get; set; }
        public string IP { get; set; }
        public bool IsIstance { get; set; }
    }
}
