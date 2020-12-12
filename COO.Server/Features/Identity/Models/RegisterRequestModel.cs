namespace COO.Server.Features.Identity.Models
{
    using FluentValidation;

    public class RegisterRequestModel
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class RegisterValidator : AbstractValidator<RegisterRequestModel>
    {
        public RegisterValidator()
        {
            RuleFor(model => model.UserName).NotNull().MinimumLength(6);
            RuleFor(model => model.Email).NotNull().EmailAddress();
            RuleFor(model => model.Password).NotNull().MinimumLength(6);
        }
    }
}
