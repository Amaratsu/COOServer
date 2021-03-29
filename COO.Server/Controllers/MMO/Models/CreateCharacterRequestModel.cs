namespace COO.Server.Controllers.MMO.Models
{
    public class CreateCharacterRequestModel
    {
        public int UserId { get; set; }
        public string SessionKey { get; set; }
        public string Name { get; set; }
        public int ClassId { get; set; }
    }
}
