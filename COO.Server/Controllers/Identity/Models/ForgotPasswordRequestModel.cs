namespace COO.Server.Features.Identity.Models
{
    using FluentValidation;

    public class ForgotPasswordRequestModel
    {
        public string Email { get; set; }
    }

    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordRequestModel>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(model => model.Email).NotNull().EmailAddress();
        }
    }
}
