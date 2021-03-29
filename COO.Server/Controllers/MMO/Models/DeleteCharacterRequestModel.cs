namespace COO.Server.Controllers.MMO.Models
{
    public class DeleteCharacterRequestModel
    {
        public int UserId { get; set; }
        public string SessionKey { get; set; }
        public int CharacterId { get; set; }
    }
}
