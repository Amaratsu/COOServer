namespace COO.Server.Data.Models
{
    public class ActiveLogin
    {
        public int UserId { get; set; }
        public int? CharacterId { get; set; }
        public string SessionKey { get; set; }
    }
}
