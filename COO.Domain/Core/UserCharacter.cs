namespace COO.Domain.Core
{
    public class UserCharacter
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int GameServerId { get; set; }
        public GameServer GameServer { get; set; }

        public int CharacterId { get; set; }
        public Character Character { get; set; }
    }
}
