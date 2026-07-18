using Api.Common.Data;

namespace Api.Features.Alerts.GetAlerts;

public static class GetAlertsEndpoint
{
    public static IEndpointRouteBuilder Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/alerts", (FakeDatabase database) => GetAlertsHandler.Handle(new GetAlertsQuery(), database));
        return app;
    }
}
