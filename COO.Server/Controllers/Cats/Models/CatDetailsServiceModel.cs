namespace COO.Server.Controllers.Cats.Models
{
    public class CatDetailsServiceModel : CatListingServiceModel
    {
        public string Description { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }
    }
}
