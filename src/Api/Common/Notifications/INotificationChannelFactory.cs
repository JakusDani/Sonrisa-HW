using Api.Common.Domain;

namespace Api.Common.Notifications;

public interface INotificationChannelFactory
{
    INotificationChannel Create(ChannelType channelType);
}
