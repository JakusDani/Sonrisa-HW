using Api.Common.Domain;
using Api.Common.Notifications;
using FluentAssertions;
using Xunit;

namespace Api.Tests.Common.Notifications;

public class InMemoryNotificationQueueTests
{
    [Fact]
    public async Task PublishAsync_Then_ReadAllAsync_ReturnsPublishedMessage()
    {
        var queue = new InMemoryNotificationQueue();
        var message = new NotificationQueueMessage(Guid.NewGuid(), Guid.NewGuid(), "Alert", ChannelType.Email, "Message");

        await queue.PublishAsync(message);

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
        NotificationQueueMessage? received = null;
        await foreach (var item in queue.ReadAllAsync(cts.Token))
        {
            received = item;
            break;
        }

        received.Should().Be(message);
    }

    [Fact]
    public async Task PublishAsync_MultipleMessages_AreReadInOrder()
    {
        var queue = new InMemoryNotificationQueue();
        var first = new NotificationQueueMessage(Guid.NewGuid(), Guid.NewGuid(), "First", ChannelType.Email, "First message");
        var second = new NotificationQueueMessage(Guid.NewGuid(), Guid.NewGuid(), "Second", ChannelType.Slack, "Second message");

        await queue.PublishAsync(first);
        await queue.PublishAsync(second);

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
        var received = new List<NotificationQueueMessage>();
        await foreach (var item in queue.ReadAllAsync(cts.Token))
        {
            received.Add(item);
            if (received.Count == 2)
            {
                break;
            }
        }

        received.Should().ContainInOrder(first, second);
    }
}
