using Api.Common.Data;

namespace Api.Features.Alerts.GetAlerts;

public static class GetAlertsEndpoint
{
    public static IEndpointRouteBuilder Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/alerts", (FakeDatabase database) => GetAlertsHandler.Handle(new GetAlertsQuery(), database));
        app.MapGet("/alerts/{id:guid}", (Guid id, FakeDatabase database) =>
        {
            var alert = GetAlertByIdHandler.Handle(new GetAlertByIdQuery(id), database);
            var result = alert is null ? Results.NotFound() : Results.Ok(alert);
            return result;
        });
        return app;
    }
}
