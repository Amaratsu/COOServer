namespace COO.Server.Features.Servers.Models
{
    public class PartyInvXRequestModel
    {
        public string Inviter { get; set; }
        public string Members { get; set; }
        public string Invitee { get; set; }
        public bool IsSent { get; set; }
        public bool IsUpdateSent { get; set; }
    }
}
