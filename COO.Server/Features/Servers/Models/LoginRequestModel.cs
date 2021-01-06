namespace COO.Server.Features.Servers.Models
{
    public class LoginRequestModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string PreDevice { get; set; }
        public int IsPlatform { get; set; }
    }
}
