

using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var client = new SmtpClient(_configuration["EmailSettings:SmtpHost"], int.Parse(_configuration["EmailSettings:SmtpPort"]))
        {
            Credentials = new NetworkCredential(_configuration["EmailSettings:SmtpUser"], _configuration["EmailSettings:SmtpPass"]),
            EnableSsl = true
        };

        await client.SendMailAsync(new MailMessage(_configuration["EmailSettings:FromEmail"], email, subject, message));
    }
}
