namespace COO.Server.Infrastructure.Services
{
    using MailKit.Net.Smtp;
    using MailKit.Security;
    using Microsoft.Extensions.Options;
    using MimeKit;
    using MimeKit.Text;
    using System.Threading.Tasks;

    public class EmailService: IEmailService
    {
        private readonly AppSettings _appSettings;

        public EmailService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task SendAsync(string to, string subject, string html, string from = null)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? _appSettings.EmailFrom));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync(_appSettings.SmtpHost, _appSettings.SmtpPort, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_appSettings.SmtpUser, _appSettings.SmtpPass);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
        }
    }
}
