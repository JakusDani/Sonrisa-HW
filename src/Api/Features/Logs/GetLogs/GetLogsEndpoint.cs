using Api.Common.Data;

namespace Api.Features.Logs.GetLogs;

public static class GetLogsEndpoint
{
    public static IEndpointRouteBuilder Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/logs", (FakeDatabase database) => GetLogsHandler.Handle(new GetLogsQuery(), database));
        return app;
    }
}
