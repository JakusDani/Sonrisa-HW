using Api.Common.Data;

namespace Api.Features.Alerts.UpdateAlert;

public static class UpdateAlertEndpoint
{
    public static IEndpointRouteBuilder Map(IEndpointRouteBuilder app)
    {
        app.MapPut("/alerts/{id:guid}", (Guid id, UpdateAlertCommand command, FakeDatabase database) =>
        {
            var response = UpdateAlertHandler.Handle(command with { Id = id }, database);
            var result = response is null ? Results.NotFound() : Results.Ok(response);
            return result;
        });
        return app;
    }
}
