using Api.Common.Data;
using Api.Common.Domain;
using Api.Common.Notifications;

namespace Api.Features.Admin.SimulateEvent;

public static class SimulateEventHandler
{
    public static async Task<SimulateEventResponse> Handle(SimulateEventCommand command, FakeDatabase database, INotificationQueue queue)
    {
        var worldEvent = new WorldEvent(Guid.NewGuid(), command.EventType, command.Message, DateTime.UtcNow);
        database.AddWorldEvent(worldEvent);

        var matchedAlerts = database.Alerts
            .Where(a => a.IsEnabled && a.EventType == worldEvent.EventType)
            .ToList();

        var queueMessages = matchedAlerts
            .SelectMany(a => a.EnabledChannels.Select(c => new NotificationQueueMessage(worldEvent.Id, a.Id, a.Name, c, worldEvent.Message)))
            .ToList();

        await Task.WhenAll(queueMessages.Select(queue.PublishAsync));

        var response = new SimulateEventResponse(matchedAlerts.Count, queueMessages.Count);
        return response;
    }
}
