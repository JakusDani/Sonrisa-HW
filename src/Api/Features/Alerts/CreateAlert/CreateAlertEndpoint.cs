using Api.Common.Data;

namespace Api.Features.Alerts.CreateAlert;

public static class CreateAlertEndpoint
{
    public static IEndpointRouteBuilder Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/alerts", (CreateAlertCommand command, FakeDatabase database) =>
            Results.Created($"/alerts/{command.Name}", CreateAlertHandler.Handle(command, database)));
        return app;
    }
}
