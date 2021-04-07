namespace COO.Server.Controllers.MMO.Models
{
    public class GetCharactersRequestModel
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public int ServerId { get; set; }
    }
}
