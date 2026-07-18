using Api.Common.Data;
using Api.Common.Notifications;

namespace Api.Features.Admin.SimulateEvent;

public static class SimulateEventEndpoint
{
    public static IEndpointRouteBuilder Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/admin/simulate", (SimulateEventCommand command, FakeDatabase database, INotificationQueue queue) =>
            SimulateEventHandler.Handle(command, database, queue));
        return app;
    }
}
