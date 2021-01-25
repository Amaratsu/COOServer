namespace COO.Server.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser<int>
    {
        public Profile Profile { get; set; }
    }
}
