using Api.Common.Data;

namespace Api.Features.Alerts.GetAlerts;

public static class GetAlertByIdHandler
{
    public static AlertResponse? Handle(GetAlertByIdQuery query, FakeDatabase database)
    {
        var alert = database.GetAlert(query.Id);
        var response = alert is null
            ? null
            : new AlertResponse(alert.Id, alert.Name, alert.EventType, alert.EnabledChannels, alert.IsEnabled);
        return response;
    }
}
