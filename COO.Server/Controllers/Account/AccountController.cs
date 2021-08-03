using System.Threading.Tasks;
using COO.Business.Logic.Account.Read.Authentication;
using COO.Business.Logic.Account.Write.ConfirmEmail;
using COO.Business.Logic.Account.Write.Registration;
using COO.Server.Controllers.Account.Models;
using COO.Server.Infrastructure.Services.Email;
using COO.Server.Infrastructure.Services.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace COO.Server.Controllers.Account
{
    public class AccountController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;
        private readonly AppSettings _appSettings;
        private readonly IEmailService _emailService;

        public AccountController(
            IIdentityService identityService,
            IOptions<AppSettings> appSettings,
            IEmailService emailService,
            IMediator mediator)
        {
            _identityService = identityService;
            _appSettings = appSettings.Value;
            _emailService = emailService;
            _mediator = mediator;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(Registration))]
        public async Task<ActionResult> Registration(RegistrationRequestModel model)
        {
            var user = await _mediator.Send(new RegistrationCommand(model.Login, model.Email, model.Password));

            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Account",
                new { userId = user.UserId, token = user.Token },
                protocol: HttpContext.Request.Scheme);

            await _emailService.SendAsync(
                to: user.Email,
                subject: "ConfirmEmail",
                html: $"Confirm registration by clicking on the link: <a href='{callbackUrl}'>link</a>"
            );

            return Ok(new { Message = $"The account was created successfully, a confirmation email {user.Email} was sent to your email.", Status = true });
        }

        [HttpGet]
        [AllowAnonymous]
        [Route(nameof(ConfirmEmail))]
        public async Task<ActionResult> ConfirmEmail(int userId, string token)
        {
            return Ok(await _mediator.Send(new ConfirmEmailCommand(userId, token)));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(Login))]
        public async Task<ActionResult> Login(string login, string password)
        {
            var user = await _mediator.Send(new AuthenticationQuery(login, password));

            var token = _identityService.GenerateJwtToken(
                user.Id.ToString(),
                user.UserName,
                _appSettings.Secret);
            return Ok(new { Token = token, user.UserName, Status = true });
        }
    }
}
