using System;

namespace COO.Domain.Core
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PaswordSalt { get; set; }
        public string Token { get; set; }
        public string IP { get; set; }
        public int CountFailedLogins { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastActivity { get; set; }

        //public int GameServerId { get; set; }
        //public GameServer GameServer { get; set; }

        //public UserCharacter UserCharacter { get; set; }
    }
}