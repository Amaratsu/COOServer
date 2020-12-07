namespace COO.Server.Infrastructure.Services
{
    using System;
    using System.ComponentModel;
    using System.Net.Mail;
    using System.Threading.Tasks;

    public class EmailService
    {
        private ValueTuple<string, string> credentials;
        public EmailService(string email, string password)
        {
            credentials.Item1 = email;
            credentials.Item2 = password;
        }

        public void SendEmail(string subject, string body, string to, string host, int port = 587, bool EnableSsl = true)
        {
            try
            {
                MailMessage message = new MailMessage() { Subject = subject, IsBodyHtml = true, Body = body };
                message.To.Add(new MailAddress(to));
                message.From = new MailAddress(credentials.Item1);

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = !string.IsNullOrWhiteSpace(host) ? host : "smtp.gmail.com";
                smtpClient.Port = port;
                smtpClient.EnableSsl = EnableSsl;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.SendCompleted += SmtpClient_SendCompleted;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(credentials.Item1, credentials.Item2);

                smtpClient.Send(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString(), "WebAPI email");
            }
        }

        private void SmtpClient_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Console.WriteLine(e.ToString(), "WebAPI email");
        }
    }
}
