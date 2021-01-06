namespace COO.Server.Features.Servers.Models
{
    public class CreateCharRequestModel
    {
        public int UserId { get; set; }
        public string PrevDevice { get; set; }
        public string Name { get; set; }
        public string Affiliation { get; set; }
        public string XP { get; set; }
        public string Status { get; set; }
        public string Inv { get; set; }
        public string Equips { get; set; }
        public string Skills { get; set; }
        public string Talents { get; set; }
        public string Appearance { get; set; }
        public string Gameplay { get; set; }
        public string Keybinds { get; set; }
        public string KeyRemap { get; set; }
        public string Chat { get; set; }
    }
}
