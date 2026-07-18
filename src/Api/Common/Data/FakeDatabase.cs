using System.Collections.Concurrent;
using Api.Common.Domain;

namespace Api.Common.Data;

public class FakeDatabase
{
    private readonly ConcurrentDictionary<Guid, Alert> _alerts = new();
    private readonly ConcurrentDictionary<Guid, WorldEvent> _worldEvents = new();
    private readonly ConcurrentDictionary<Guid, NotificationLog> _notificationLogs = new();

    public IReadOnlyCollection<Alert> Alerts => _alerts.Values.ToList();
    public IReadOnlyCollection<WorldEvent> WorldEvents => _worldEvents.Values.ToList();
    public IReadOnlyCollection<NotificationLog> NotificationLogs => _notificationLogs.Values.ToList();

    public Alert AddAlert(Alert alert)
    {
        var result = _alerts.AddOrUpdate(alert.Id, alert, (_, _) => alert);
        return result;
    }

    public bool RemoveAlert(Guid id)
    {
        var removed = _alerts.TryRemove(id, out _);
        return removed;
    }

    public Alert? UpdateAlert(Guid id, Func<Alert, Alert> updater)
    {
        var updated = _alerts.TryGetValue(id, out var existing)
            ? _alerts.AddOrUpdate(id, existing, (_, current) => updater(current))
            : null;
        return updated;
    }

    public Alert? GetAlert(Guid id)
    {
        var found = _alerts.TryGetValue(id, out var alert) ? alert : null;
        return found;
    }

    public WorldEvent AddWorldEvent(WorldEvent worldEvent)
    {
        var result = _worldEvents.AddOrUpdate(worldEvent.Id, worldEvent, (_, _) => worldEvent);
        return result;
    }

    public NotificationLog AddNotificationLog(NotificationLog log)
    {
        var result = _notificationLogs.AddOrUpdate(log.Id, log, (_, _) => log);
        return result;
    }
}
