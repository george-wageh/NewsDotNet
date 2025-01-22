using Microsoft.Extensions.Options;
using NewsWebApi.ConfigurationModels;
using System.Net.Mail;
using System.Text;

namespace NewsWebApi.Services
{
    public class EmailService
    {
        private readonly EmailConfiguration _emailConfiguration;

        public EmailService(IOptions<EmailConfiguration> emailConfiguration)
        {
            _emailConfiguration = emailConfiguration.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            string to = toEmail;
            string from = _emailConfiguration.SenderEmail;
            MailMessage message = new MailMessage(from, to);

            string mailbody = body;
            message.Subject = subject;
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient(_emailConfiguration.SmtpServer, _emailConfiguration.Port); //Gmail smtp    
            System.Net.NetworkCredential basicCredential1 = new
            System.Net.NetworkCredential(_emailConfiguration.Username, _emailConfiguration.Password);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;

            client.Credentials = basicCredential1;
            try
            {
                client.Send(message);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
