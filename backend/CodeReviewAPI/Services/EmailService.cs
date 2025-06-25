using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace CodeReviewAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendVerificationEmailAsync(string toEmail, string token)
        {
            var message = new MimeMessage();

            // Gönderen adres
            message.From.Add(MailboxAddress.Parse(_config["Email:From"]));

            // Alýcý adres
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = "Verify your email";

            // Doðrulama baðlantýsý
            var verificationLink = $"{_config["Frontend:BaseUrl"]}/verify-email?token={token}";

            // Mail içeriði
            message.Body = new TextPart("plain")
            {
                Text = $"Please verify your email by clicking the following link: {verificationLink}"
            };

            using var smtp = new SmtpClient();

            // TLS ile baðlantý kur (port 587 için)
            await smtp.ConnectAsync(
                _config["Email:SmtpServer"],
                int.Parse(_config["Email:Port"]),
                SecureSocketOptions.StartTls
            );

            // Kimlik doðrulama
            await smtp.AuthenticateAsync(
                _config["Email:Username"],
                _config["Email:Password"]
            );

            // Mail gönderimi
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }
}
