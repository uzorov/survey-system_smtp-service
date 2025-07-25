using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MassMailService.Services
{
    public class EmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromAddress;

        public EmailService(string host, int port, string username, string password, string fromAddress)
        {
            _smtpClient = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };
            _fromAddress = fromAddress;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            Console.WriteLine($"[SMTP] Попытка отправки письма: to={to}, subject={subject}");
            try
            {
                var mail = new MailMessage(_fromAddress, to, subject, body)
                {
                    IsBodyHtml = true
                };
                Console.WriteLine($"[SMTP] SMTP-сервер: {_smtpClient.Host}:{_smtpClient.Port}, SSL: {_smtpClient.EnableSsl}");
                await _smtpClient.SendMailAsync(mail);
                Console.WriteLine($"[SMTP] Письмо успешно отправлено: to={to}");
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"[SMTP][ERROR] SmtpException: {smtpEx.Message}");
                if (smtpEx.InnerException != null)
                    Console.WriteLine($"[SMTP][ERROR] Внутренняя ошибка: {smtpEx.InnerException.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SMTP][ERROR] Exception: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"[SMTP][ERROR] Внутренняя ошибка: {ex.InnerException.Message}");
            }
        }
    }
}
