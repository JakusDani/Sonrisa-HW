using Api.Common.Domain;

namespace Api.Common.Notifications;

public interface INotificationQueue
{
    Task PublishAsync(NotificationQueueMessage message);

    IAsyncEnumerable<NotificationQueueMessage> ReadAllAsync(CancellationToken cancellationToken);
}
