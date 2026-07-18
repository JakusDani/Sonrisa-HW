using Api.Common.Domain;

namespace Api.Common.Notifications;

public interface INotificationChannel
{
    Task<NotificationResult> SendAsync(NotificationQueueMessage message);
}
