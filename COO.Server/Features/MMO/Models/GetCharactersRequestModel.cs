namespace COO.Server.Features.MMO.Models
{
    public class GetCharactersRequestModel
    {
        public int UserId { get; set; }
        public string SessionKey { get; set; }
    }
}
