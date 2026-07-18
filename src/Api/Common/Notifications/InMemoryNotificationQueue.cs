using System.Threading.Channels;
using Api.Common.Domain;

namespace Api.Common.Notifications;

public class InMemoryNotificationQueue : INotificationQueue
{
    private readonly Channel<NotificationQueueMessage> _channel = Channel.CreateUnbounded<NotificationQueueMessage>();

    public async Task PublishAsync(NotificationQueueMessage message)
    {
        await _channel.Writer.WriteAsync(message);
    }

    public IAsyncEnumerable<NotificationQueueMessage> ReadAllAsync(CancellationToken cancellationToken)
    {
        var reader = _channel.Reader.ReadAllAsync(cancellationToken);
        return reader;
    }
}
