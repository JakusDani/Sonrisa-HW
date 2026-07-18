using Api.Common.Data;

namespace Api.Features.Alerts.CreateAlert;

public static class CreateAlertEndpoint
{
    public static IEndpointRouteBuilder Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/alerts", (CreateAlertCommand command, FakeDatabase database) =>
        {
            var response = CreateAlertHandler.Handle(command, database);
            var result = Results.Created($"/alerts/{response.Id}", response);
            return result;
        });
        return app;
    }
}
