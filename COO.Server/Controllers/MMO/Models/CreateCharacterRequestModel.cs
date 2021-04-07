namespace COO.Server.Controllers.MMO.Models
{
    public class CreateCharacterRequestModel
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        // 0 - male, 1 - female
        public int Gender { get; set; }
        public int RaceId { get; set; }
        public int ClassId { get; set; }
        public int ServerId { get; set; }
    }
}
