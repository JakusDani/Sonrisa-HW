using Api.Common.Domain;

namespace Api.Common.Notifications;

public class EmailNotificationChannel : INotificationChannel
{
    public Task<NotificationResult> SendAsync(NotificationQueueMessage message)
    {
        Console.WriteLine($"[Email] Sending to alert '{message.AlertName}': {message.Message}");
        var result = Task.FromResult(new NotificationResult(true));
        return result;
    }
}
