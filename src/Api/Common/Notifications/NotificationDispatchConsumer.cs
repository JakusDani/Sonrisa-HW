using Api.Common.Data;
using Api.Common.Domain;

namespace Api.Common.Notifications;

public class NotificationDispatchConsumer : BackgroundService
{
    private readonly INotificationQueue _queue;
    private readonly INotificationChannelFactory _channelFactory;
    private readonly FakeDatabase _database;
    private readonly ILogger<NotificationDispatchConsumer> _logger;

    public NotificationDispatchConsumer(
        INotificationQueue queue,
        INotificationChannelFactory channelFactory,
        FakeDatabase database,
        ILogger<NotificationDispatchConsumer> logger)
    {
        _queue = queue;
        _channelFactory = channelFactory;
        _database = database;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var message in _queue.ReadAllAsync(stoppingToken))
        {
            await _DispatchAsync(message);
        }
    }

    private async Task _DispatchAsync(NotificationQueueMessage message)
    {
        var channel = _channelFactory.Create(message.Channel);
        var result = await channel.SendAsync(message);
        var log = new NotificationLog(
            Guid.NewGuid(),
            message.AlertId,
            message.AlertName,
            message.Channel,
            message.Message,
            result.Success ? NotificationStatus.Success : NotificationStatus.Failed,
            DateTime.UtcNow);

        _database.AddNotificationLog(log);
        _logger.LogInformation(
            "Dispatched notification for alert '{AlertName}' via {Channel}: {Status}",
            message.AlertName,
            message.Channel,
            log.Status);
    }
}
