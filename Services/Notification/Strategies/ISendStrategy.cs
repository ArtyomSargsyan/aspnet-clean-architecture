public interface ISendStrategy
{
    Task SendAsync(string recipient, string message);
}
