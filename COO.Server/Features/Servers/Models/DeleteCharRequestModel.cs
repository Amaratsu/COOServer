namespace COO.Server.Features.Servers.Models
{
    public class DeleteCharRequestModel
    {
        public int UserId { get; set; }
        public string PrevDevice { get; set; }
        public string Name { get; set; }
    }
}
