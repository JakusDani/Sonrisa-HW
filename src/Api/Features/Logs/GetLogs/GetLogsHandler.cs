using Api.Common.Data;

namespace Api.Features.Logs.GetLogs;

public static class GetLogsHandler
{
    public static IReadOnlyCollection<NotificationLogResponse> Handle(GetLogsQuery query, FakeDatabase database)
    {
        var responses = database.NotificationLogs
            .OrderByDescending(l => l.TimestampUtc)
            .Select(l => new NotificationLogResponse(l.Id, l.AlertId, l.AlertName, l.Channel, l.Message, l.Status, l.TimestampUtc))
            .ToList();
        return responses;
    }
}
