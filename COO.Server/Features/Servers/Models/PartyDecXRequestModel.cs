namespace COO.Server.Features.Servers.Models
{
    public class PartyDecXRequestModel
    {
        public bool IsAccepted { get; set; }
        public string Inviter { get; set; }
        public string Members { get; set; }
        public string Invitee { get; set; }
        public bool IsCancel { get; set; }
        public string NewLeader { get; set; }
    }
}
