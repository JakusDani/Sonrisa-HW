using Api.Common.Data;

namespace Api.Features.Alerts.DeleteAlert;

public static class DeleteAlertEndpoint
{
    public static IEndpointRouteBuilder Map(IEndpointRouteBuilder app)
    {
        app.MapDelete("/alerts/{id:guid}", (Guid id, FakeDatabase database) =>
        {
            var response = DeleteAlertHandler.Handle(new DeleteAlertCommand(id), database);
            var result = response.Deleted ? Results.Ok(response) : Results.NotFound(response);
            return result;
        });
        return app;
    }
}
