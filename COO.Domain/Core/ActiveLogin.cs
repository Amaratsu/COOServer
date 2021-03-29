namespace COO.Domain.Core
{
    public class ActiveLogin
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? CharacterId { get; set; }
        public string SessionKey { get; set; }
    }
}
