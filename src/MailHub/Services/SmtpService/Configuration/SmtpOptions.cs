namespace MailHub.Services.MailService.Configuration
{
    public class SmtpOptions
    {
        public string ServerName { get; set; }
        public string[] AllowedDomains { get; set; }
    }
}