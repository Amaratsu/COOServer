namespace COO.Server.Features.MMO
{
    using COO.Server.Infrastructure.Services;
    using Microsoft.Extensions.Options;

    public class MMOController : ApiController
    {
        private readonly IMMOService mmoService;
        private readonly AppSettings appSettings;
        private readonly IEmailService emailService;

        public MMOController(
                IMMOService mmoService,
                IOptions<AppSettings> appSettings,
                IEmailService emailService
            )
        {
            this.mmoService = mmoService;
            this.appSettings = appSettings.Value;
            this.emailService = emailService;
        }
    }
}
