namespace COO.Server.Features.MMO.Models
{
    public class DeleteCharacterRequestModel
    {
        public int UserId { get; set; }
        public string SessionKey { get; set; }
        public int CharId { get; set; }
    }
}
