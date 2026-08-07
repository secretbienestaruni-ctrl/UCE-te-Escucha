using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Plataforma_Virtual.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnviarCorreoAsync(string asunto, string mensaje)
        {
            string emailOrigen = _configuration["EmailSettings:Email"]
                ?? throw new Exception("No se configuró EmailSettings:Email");

            string password = _configuration["EmailSettings:Password"]
                ?? throw new Exception("No se configuró EmailSettings:Password");

            string host = _configuration["EmailSettings:Host"]
                ?? throw new Exception("No se configuró EmailSettings:Host");

            int port = int.Parse(_configuration["EmailSettings:Port"] ?? "587");

            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("UCE te Escucha", emailOrigen));
            email.To.Add(MailboxAddress.Parse(emailOrigen));

            email.Subject = asunto;

            email.Body = new TextPart("html")
            {
                Text = mensaje
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(host, port, SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(emailOrigen, password);

            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }
    }
}