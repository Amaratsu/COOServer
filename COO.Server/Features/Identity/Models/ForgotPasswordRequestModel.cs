namespace COO.Server.Features.Identity.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ForgotPasswordRequestModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
