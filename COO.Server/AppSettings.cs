namespace COO.Server
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string EmailFrom { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
        public string ClientUrl { get; set; }
    }
}
