using System.Threading.Tasks;
using COO.Server.Controllers.Identity.Models;
using COO.Server.Infrastructure.Helpers;
using COO.Server.Infrastructure.Services;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace COO.Server.Controllers.Identity
{
    public class IdentityController : ApiController
    {
        private readonly UserManager<User> userManager;
        private readonly IIdentityService identity;
        private readonly AppSettings appSettings;
        private readonly IEmailService emailService;

        public IdentityController(
            UserManager<User> userManager,
            IIdentityService identity,
            IOptions<AppSettings> appSettings,
            IEmailService emailService)
        {
            this.userManager = userManager;
            this.identity = identity;
            this.appSettings = appSettings.Value;
            this.emailService = emailService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(Register))]
        public async Task<ActionResult> Register(RegisterRequestModel model)
        {
            RegisterValidator validator = new RegisterValidator();
            ValidationResult validateResult = validator.Validate(model);

            if (validateResult.IsValid)
            {
                var user = new User
                {
                    Email = model.Email,
                    UserName = model.UserName
                };

                var newUser = await this.userManager.CreateAsync(user, model.Password);
                if (newUser.Succeeded)
                {
                    var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);

                    var callbackUrl = Url.Action(
                            "ConfirmEmail",
                            "Identity",
                            new { userId = user.Id, code = code },
                            protocol: HttpContext.Request.Scheme);

                    await this.emailService.SendAsync(
                        to: user.Email,
                        subject: "ConfirmEmail",
                        html: $"Confirm registration by clicking on the link: <a href='{callbackUrl}'>link</a>"
                        );

                    return Ok(new { status = "To complete the registration, check your email and follow the link provided in the letter" });
                }
                else
                {
                    var errorMessage = "";
                    foreach (var error in newUser.Errors)
                    {
                        errorMessage = error.Description;
                    }

                    throw new AppException(errorMessage);
                }
            } 
            else
            {
                throw new AppException(validateResult.Errors[0].ErrorMessage);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route(nameof(ConfirmEmail))]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                throw new AppException("Link is incorrect");
            }
            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new AppException("Link is incorrect");
            }
            var result = await this.userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return Redirect($"{this.appSettings.ClientUrl}/login");
            }
            else
            {
                throw new AppException("Link is incorrect");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(ForgotPassword))]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordRequestModel model)
        {
            ForgotPasswordValidator validator = new ForgotPasswordValidator();
            ValidationResult validateResult = validator.Validate(model);

            if (validateResult.IsValid)
            {
                var user = await this.userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await this.userManager.IsEmailConfirmedAsync(user)))
                {
                    // the user with the given email may not be present in the database
                    // however, we print a standard message to hide
                    // presence or absence of a user in the database
                    return Ok(new { status = "To reset your password, follow the link in the letter sent to your email." });
                }

                var code = await this.userManager.GeneratePasswordResetTokenAsync(user);

                var callbackUrl = $"{this.appSettings.ClientUrl}/reset-password?code={code}";

                await this.emailService.SendAsync(
                    model.Email,
                    "Reset Password",
                    $"To reset your password, follow the link: <a href='{callbackUrl}'>link</a>");

                return Ok( new { status = "To reset your password, follow the link in the letter sent to your email." });
            }
            else
            {
                throw new AppException(validateResult.Errors[0].ErrorMessage);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(ResetPassword))]
        public async Task<ActionResult> ResetPassword(ResetPasswordRequestModel model)
        {
            ResetPasswordValidator validator = new ResetPasswordValidator();
            ValidationResult validateResult = validator.Validate(model);

            if (validateResult.IsValid)
            {
                var user = await this.userManager.FindByEmailAsync(model.Email);
                
                if (user == null)
                {
                    return Ok( new { status = "Your password has been reset. To enter the application, follow the" });
                }

                var code = model.Code.Replace(" ", "+");

                var result = await this.userManager.ResetPasswordAsync(user, code, model.Password);

                if (result.Succeeded)
                {
                    return Ok(new { status = "Your password has been reset. To enter the application, follow the" });
                }
                else
                {
                    throw new AppException("Token is not valid");
                }
            }
            else
            {
                throw new AppException(validateResult.Errors[0].ErrorMessage);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(Login))]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel model)
        {
            LoginValidator validator = new LoginValidator();
            ValidationResult validateResult = validator.Validate(model);

            if (validateResult.IsValid)
            {

                var user = await this.userManager.FindByNameAsync(model.Login);
                if (user == null)
                {
                    throw new AppException("UserName or password is incorrect");
                }

                if (!await this.userManager.CheckPasswordAsync(user, model.Password))
                {
                    throw new AppException("UserName or password is incorrect");
                }

                if (!await this.userManager.IsEmailConfirmedAsync(user))
                {
                    throw new AppException("UserName or password is incorrect");
                }

                var token = this.identity.GenerateJwtToken(
                    user.Id.ToString(),
                    user.UserName,
                    this.appSettings.Secret);

                return new LoginResponseModel
                {
                    UserName = user.UserName,
                    Token = token
                };
            }
            else
            {
                throw new AppException(validateResult.Errors[0].ErrorMessage);
            }
        }
    }
}
