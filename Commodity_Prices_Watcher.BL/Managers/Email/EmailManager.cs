
using Commodity_Prices_Watcher.DAL;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace Commodity_Prices_Watcher.BL
{
    public class EmailManager : IEmailManager
    {
        private readonly EmailConfiguration _emailConfiguration;
        public EmailManager(IOptions<EmailConfiguration> emailConfiguration)
        {
            _emailConfiguration = emailConfiguration.Value;
        }
        public async Task<bool> SendEmailAsync(List<string> recieversEmail, string subject, string message)
        {
            try
            {
                SmtpClient smtpClient = new(_emailConfiguration.SmtpServer, _emailConfiguration.Port)
                {
                    EnableSsl = false,
                    Credentials = new NetworkCredential(_emailConfiguration.UserName, _emailConfiguration.Password)
                };

                MailMessage mail = new MailMessage();

                mail.From = new MailAddress(_emailConfiguration.EmailSender, "IDSC-Commodity prices watcher");
                mail.Subject = subject;
                mail.Body = message;

                foreach (var email in recieversEmail)
                {
                    mail.To.Add(email);
                }

                await smtpClient.SendMailAsync(mail);

                return true;
            }
            catch (Exception ex)
            {
                var messageException = ex.Message;
                return false;
            }
        }
    }
}
