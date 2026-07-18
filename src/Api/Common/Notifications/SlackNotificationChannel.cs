using Api.Common.Domain;

namespace Api.Common.Notifications;

public class SlackNotificationChannel : INotificationChannel
{
    public Task<NotificationResult> SendAsync(NotificationQueueMessage message)
    {
        Console.WriteLine($"[Slack] Sending to alert '{message.AlertName}': {message.Message}");
        var result = Task.FromResult(new NotificationResult(true));
        return result;
    }
}
