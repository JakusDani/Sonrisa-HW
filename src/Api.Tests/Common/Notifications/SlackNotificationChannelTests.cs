using Api.Common.Domain;
using Api.Common.Notifications;
using Shouldly;
using Xunit;

namespace Api.Tests.Common.Notifications;

public class SlackNotificationChannelTests
{
    [Fact]
    public async Task SendAsync_ReturnsSuccess()
    {
        var channel = new SlackNotificationChannel();
        var message = new NotificationQueueMessage(Guid.NewGuid(), Guid.NewGuid(), "Alert", ChannelType.Slack, "Message");

        var result = await channel.SendAsync(message);

        result.Success.ShouldBeTrue();
    }
}
