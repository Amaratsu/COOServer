namespace COO.Server.Infrastructure.Services
{
    using System.Threading.Tasks;

    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string html, string from = null);
    }
}
