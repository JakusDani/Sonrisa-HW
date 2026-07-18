using System.Collections.Concurrent;
using Api.Common.Domain;
using Api.Common.Notifications;

namespace Api.Tests.TestDoubles;

public class RecordingNotificationQueue : INotificationQueue
{
    private readonly ConcurrentQueue<NotificationQueueMessage> _published = new();

    public IReadOnlyCollection<NotificationQueueMessage> PublishedMessages => _published.ToList();

    public Task PublishAsync(NotificationQueueMessage message)
    {
        _published.Enqueue(message);
        return Task.CompletedTask;
    }

    public async IAsyncEnumerable<NotificationQueueMessage> ReadAllAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (_published.TryDequeue(out var message))
        {
            yield return message;
            await Task.Yield();
        }
    }
}
