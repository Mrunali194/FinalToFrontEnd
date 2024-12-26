using System.Net;
using System.Net.Mail;
using Demo.Dtos;

namespace Demo.Repository
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(MailRequestDTO mailRequest)
        {
            try
            {
                // Get SMTP settings from configuration
                var smtpHost = _configuration["MailSettings:SmtpHost"];
                var smtpPort = int.Parse(_configuration["MailSettings:SmtpPort"]);
                var smtpUser = _configuration["MailSettings:SmtpUser"];
                var smtpPass = _configuration["MailSettings:SmtpPass"];
                var fromEmail = _configuration["MailSettings:FromEmail"];

                // Create email message
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = mailRequest.Subject,
                    Body = mailRequest.Body,
                    IsBodyHtml = true // Enable HTML content if required
                };

                mailMessage.To.Add(mailRequest.ToEmail);

                // Configure SMTP client
                using var smtpClient = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUser, smtpPass),
                    EnableSsl = true // Use SSL if required by the SMTP server
                };

                // Send email
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email. Error: {ex.Message}");
            }
        }
    }
}
