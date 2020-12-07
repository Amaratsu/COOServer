namespace COO.Server.Features.Identity
{
    using System.Threading.Tasks;
    using COO.Server.Infrastructure.Services;
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Models;

    public class IdentityController : ApiController
    {
        private readonly UserManager<User> userManager;
        private readonly IIdentityService identity;
        private readonly AppSettings appSettings;

        public IdentityController(
            UserManager<User> userManager,
            IIdentityService identity,
            IOptions<AppSettings> appSettings)
        {
            this.userManager = userManager;
            this.identity = identity;
            this.appSettings = appSettings.Value;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(Register))]
        public async Task<ActionResult> Register(RegisterRequestModel model)
        {
            var user = new User
            {
                Email = model.Email,
                UserName = model.UserName
            };

            var result = await this.userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);

                var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Identity",
                        new { userId = user.Id, code = code },
                        protocol: HttpContext.Request.Scheme);

                EmailService emailService = new EmailService(this.appSettings.EmailSender, this.appSettings.PasswordSender);

                emailService.SendEmail(
                    "Confirm your account",
                    $"Confirm registration by clicking on the link: <a href='{callbackUrl}'>link</a>",
                    user.Email,
                    "smtp.gmail.com"
                );

                return Ok("To complete the registration, check your email and follow the link provided in the letter");
            }

            return BadRequest(result.Errors);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route(nameof(ConfirmEmail))]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest();
            }
            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Unauthorized();
            }
            var result = await this.userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return Ok();
            else
                return Unauthorized();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(Login))]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel model)
        {
            var user = await this.userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return Unauthorized();
            }

            if (!await this.userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized();
            }

            if (!await this.userManager.IsEmailConfirmedAsync(user))
            {
                //ModelState.AddModelError(string.Empty, "You have not confirmed your email");
                return Unauthorized();
            }

            var token = this.identity.GenerateJwtToken(
                user.Id,
                user.UserName,
                this.appSettings.Secret);

            return new LoginResponseModel
            {
                Token = token
            };
        }
    }
}
