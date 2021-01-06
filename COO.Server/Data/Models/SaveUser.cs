namespace COO.Server.Data.Models
{
    public class SaveUser
    {
        //public Profile Profile { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Verification { get; set; }
        public int BanTime { get; set; }
        public string PrevIP { get; set; }
        public string PrevLogin { get; set; }
        public string PrevDevice { get; set; }
        public string FavServers { get; set; }
        public int CharLimit { get; set; }
        public string FriendList { get; set; }
        public string BlockedList { get; set; }
        public string Privacy { get; set; }
        public string BankInv { get; set; }
        //public IEnumerable<Cat> Cats { get; } = new HashSet<Cat>();
    }
}
