namespace COO.Server.Controllers.Account.Models
{
    public class ResetPasswordRequestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }
    }
}
