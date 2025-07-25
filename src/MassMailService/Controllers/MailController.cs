using MassMailService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace MassMailService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MailController : ControllerBase
    {
        private readonly ClientRepository _clientRepo;
        private readonly EmailService _emailService;
        private readonly string _templatePath;

        public MailController(IConfiguration config)
        {
            var connectionString = config["ConnectionStrings:Postgres"];
            var smtpHost = config["Smtp:Host"];
            var smtpPort = int.Parse(config["Smtp:Port"] ?? "587");
            var smtpUser = config["Smtp:User"];
            var smtpPass = config["Smtp:Pass"];
            var fromAddress = config["Smtp:From"];
            _clientRepo = new ClientRepository(connectionString);
            _emailService = new EmailService(smtpHost, smtpPort, smtpUser, smtpPass, fromAddress);
            _templatePath = "Templates/EmailTemplate.html";
        }

        [HttpPost("send-test")] 
        public async Task<IActionResult> SendTestMail()
        {
            var clients = await _clientRepo.GetClientsAsync();
            var template = await System.IO.File.ReadAllTextAsync(_templatePath);
            foreach (var client in clients)
            {
                var body = template.Replace("{{Name}}", client.Name);
                await _emailService.SendEmailAsync(client.Email, "Тестовая рассылка", body);
            }
            return Ok($"Sent to {clients.Count} clients");
        }
    }
}
