namespace COO.Server.Features.MMO.Models
{
    public class CheckClientRequestModel
    {
        public int UserId { get; set; }
        public string SessionKey { get; set; }
        public int CharId { get; set; }
    }
}
