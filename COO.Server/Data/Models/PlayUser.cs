namespace COO.Server.Data.Models
{
    public class PlayUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public bool IsLogin { get; set; }
        public string MainIP { get; set; }
        public string InstanceIP { get; set; }
        public string PotentialGI { get; set; }
        public string Alert { get; set; }
        public string CurrentChar { get; set; }
        public string CurrentParty { get; set; }
        public string Leader { get; set; }
        public string XServerMessages { get; set; }
        public string GIReady { get; set; }
    }
}
