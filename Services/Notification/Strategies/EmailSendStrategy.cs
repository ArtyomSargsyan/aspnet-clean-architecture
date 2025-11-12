
public class EmailSendStrategy : ISendStrategy
{
    private readonly IEmailService _emailService;

    public EmailSendStrategy(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task SendAsync(string recipient, string message)
    {
        await _emailService.SendEmailAsync(recipient, "Notification", message);
    }
}
