using Api.Common.Domain;
using Api.Common.Notifications;
using Shouldly;
using Xunit;

namespace Api.Tests.Common.Notifications;

public class EmailNotificationChannelTests
{
    [Fact]
    public async Task SendAsync_ReturnsSuccess()
    {
        var channel = new EmailNotificationChannel();
        var message = new NotificationQueueMessage(Guid.NewGuid(), Guid.NewGuid(), "Alert", ChannelType.Email, "Message");

        var result = await channel.SendAsync(message);

        result.Success.ShouldBeTrue();
    }
}
