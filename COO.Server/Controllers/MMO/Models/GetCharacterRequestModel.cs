namespace COO.Server.Controllers.MMO.Models
{
    public class GetCharacterRequestModel
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public int CharacterId { get; set; }
        public int ServerId { get; set; }
    }
}
