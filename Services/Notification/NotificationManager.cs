public class NotificationManager : INotificationManager
{
    private readonly ISendStrategyFactory _factory;

    public NotificationManager(ISendStrategyFactory factory)
    {
        _factory = factory;
    }

    public async Task SendAsync(NotificationDto dto)
    {
        var strategy = _factory.Create(dto.Channel);
        await strategy.SendAsync(dto.Recipient, dto.Message);
    }
}
