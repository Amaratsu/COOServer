using FluentValidation;

namespace COO.Server.Controllers.Identity.Models
{
    public class ResetPasswordRequestModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ResetPasswordValidator : AbstractValidator<ResetPasswordRequestModel>
    {
        public ResetPasswordValidator()
        {
            RuleFor(model => model.Email).NotNull().EmailAddress();
            RuleFor(model => model.Password).NotNull().MinimumLength(6);
            RuleFor(model => model.ConfirmPassword).Equal(model => model.Password);
        }
    }
}
