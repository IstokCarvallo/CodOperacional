using APISegura.Common;
using APISegura.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace APISegura.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly IConfiguration _config;

        public SmtpEmailService(IOptions<EmailSettings> settings, IConfiguration config)
        {
            _settings = settings.Value;
            _config = config;
        }

        public async Task SendPasswordReset(string email, string token)
        {
            var baseUrl = _config["Frontend:BaseUrl"];

            // URL que luego usará el frontend
            var resetLink = $"{baseUrl}/resetpassword?token={Uri.EscapeDataString(token)}";

            var subject = "Recuperación de contraseña";

            var body = $@"
            <h3>Recuperación de contraseña</h3>
            <p>Haz clic en el siguiente enlace:</p>
            <a href='{resetLink}'>Restablecer contraseña</a>
            <p>Este enlace expira en 15 minutos.</p>";

            using var client = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort)
            {
                Credentials = new NetworkCredential(_settings.Username, _settings.Password),
                EnableSsl = _settings.EnableSsl
            };

            var mail = new MailMessage
            {
                From = new MailAddress(_settings.From),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mail.To.Add(email);

            await client.SendMailAsync(mail);
        }
    }
}
