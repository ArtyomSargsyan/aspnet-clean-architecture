using System.Net;
using System.Net.Mail;

public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration _config;

    public SmtpEmailService(IConfiguration config) => _config = config;

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var section = _config.GetSection("SmtpSettings");
        var portValue = section["Port"];
        if (!int.TryParse(portValue, out var port))
        {
            throw new InvalidOperationException("SmtpSettings: 'Port' is missing or not a valid integer.");
        }

        using var client = new SmtpClient(section["Host"], port)
        {
            Credentials = new NetworkCredential(section["Username"], section["Password"]),
            EnableSsl = true
        };

        var from = section["From"] ?? throw new InvalidOperationException("SmtpSettings: 'From' is missing or empty.");
        var mail = new MailMessage(from, to, subject, body)
        {
            IsBodyHtml = true
        };

        await client.SendMailAsync(mail);
    }
}
