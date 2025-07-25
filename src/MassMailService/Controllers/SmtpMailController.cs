using MassMailService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace MassMailService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SmtpMailController : ControllerBase
    {
        private readonly IConfiguration _config;
        public SmtpMailController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("send-test")]
        public async Task<IActionResult> SendTestMail([FromBody] string toEmail)
        {
            var smtpHost = _config["Smtp:Host"];
            var smtpPort = int.Parse(_config["Smtp:Port"] ?? "587");
            var smtpUser = _config["Smtp:User"];
            var smtpPass = _config["Smtp:Pass"];
            var fromAddress = _config["Smtp:From"];

            if (string.IsNullOrWhiteSpace(smtpHost) || 
                string.IsNullOrWhiteSpace(smtpUser) ||
                string.IsNullOrWhiteSpace(smtpPass) ||
                string.IsNullOrWhiteSpace(fromAddress))
            {
                return BadRequest("SMTP host configuration is missing.");
            }

            var emailService = new EmailService(smtpHost, smtpPort, smtpUser, smtpPass, fromAddress);
            var to = smtpUser;
            var subject = "Тестовая рассылка через SMTP";
            var body = "<p>Это тестовое письмо, отправленное через SMTP Mailtrap.</p>";

            await emailService.SendEmailAsync(to, subject, body);
            return Ok($"Письмо отправлено через SMTP на {to}");
        }
    }
}
