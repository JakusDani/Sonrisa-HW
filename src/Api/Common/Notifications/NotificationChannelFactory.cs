using Api.Common.Domain;

namespace Api.Common.Notifications;

public class NotificationChannelFactory : INotificationChannelFactory
{
    private readonly Dictionary<ChannelType, Func<INotificationChannel>> _channelMap;

    public NotificationChannelFactory()
    {
        _channelMap = new Dictionary<ChannelType, Func<INotificationChannel>>
        {
            [ChannelType.Email] = () => new EmailNotificationChannel(),
            [ChannelType.Slack] = () => new SlackNotificationChannel()
        };
    }

    public INotificationChannel Create(ChannelType channelType)
    {
        var found = _channelMap.TryGetValue(channelType, out var factory)
            ? factory()
            : throw new NotSupportedException($"No notification channel registered for '{channelType}'.");
        return found;
    }
}
