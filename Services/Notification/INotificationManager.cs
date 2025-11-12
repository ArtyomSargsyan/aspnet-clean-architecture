public interface INotificationManager
{
    Task SendAsync(NotificationDto dto);
}
