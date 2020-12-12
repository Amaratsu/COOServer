namespace COO.Server.Features.Identity.Models
{
    using FluentValidation;

    public class LoginRequestModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public class LoginValidator : AbstractValidator<LoginRequestModel>
    {
        public LoginValidator()
        {
            RuleFor(model => model.UserName).NotNull().MinimumLength(6);
            RuleFor(model => model.Password).NotNull().EmailAddress();
        }
    }
}
