namespace COO.Server.Features.Servers.Models
{
    public class PartyLeaveXRequestModel
    {
        public string Leaver { get; set; }
        public string NewLeader { get; set; }
        public string Members { get; set; }
    }
}
