using Api.Common.Data;

namespace Api.Features.Alerts.GetAlerts;

public static class GetAlertsHandler
{
    public static IReadOnlyCollection<AlertResponse> Handle(GetAlertsQuery query, FakeDatabase database)
    {
        var responses = database.Alerts
            .Select(a => new AlertResponse(a.Id, a.Name, a.EventType, a.EnabledChannels, a.IsEnabled))
            .ToList();
        return responses;
    }
}
