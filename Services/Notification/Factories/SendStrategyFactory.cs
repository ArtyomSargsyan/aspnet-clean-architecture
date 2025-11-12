public class SendStrategyFactory : ISendStrategyFactory
{
    private readonly IServiceProvider _serviceProvider;

    public SendStrategyFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ISendStrategy Create(string channel)
    {
        return channel?.ToLower() switch
        {
            "email" => _serviceProvider.GetRequiredService<EmailSendStrategy>(),
            _ => throw new ArgumentException($"Unsupported channel: {channel}")
        };
    }
}
