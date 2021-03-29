using FluentValidation;

namespace COO.Server.Controllers.Identity.Models
{
    public class LoginRequestModel
    {
        public string Login { get; set; }

        public string Password { get; set; }
    }

    public class LoginValidator : AbstractValidator<LoginRequestModel>
    {
        public LoginValidator()
        {
            RuleFor(model => model.Login).NotNull().MinimumLength(6);
            RuleFor(model => model.Password).NotNull().MinimumLength(6);
        }
    }
}
