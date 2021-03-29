namespace COO.Server.Controllers.MMO.Models
{
    public class CheckClientRequestModel
    {
        public int UserId { get; set; }
        public string SessionKey { get; set; }
        public int CharacterId { get; set; }
    }
}
