namespace COO.Server.Features.Identity.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ResetPasswordRequestModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
