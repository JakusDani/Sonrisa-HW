using Api.Common.Domain;
using Api.Common.Notifications;
using Shouldly;
using Xunit;

namespace Api.Tests.Common.Notifications;

public class NotificationChannelFactoryTests
{
    [Fact]
    public void Create_ReturnsEmailChannel_WhenChannelTypeIsEmail()
    {
        var factory = new NotificationChannelFactory();

        var channel = factory.Create(ChannelType.Email);

        channel.ShouldBeOfType<EmailNotificationChannel>();
    }

    [Fact]
    public void Create_ReturnsSlackChannel_WhenChannelTypeIsSlack()
    {
        var factory = new NotificationChannelFactory();

        var channel = factory.Create(ChannelType.Slack);

        channel.ShouldBeOfType<SlackNotificationChannel>();
    }

    [Fact]
    public void Create_Throws_WhenChannelTypeIsUnsupported()
    {
        var factory = new NotificationChannelFactory();

        Should.Throw<NotSupportedException>(() => factory.Create((ChannelType)999));
    }
}
