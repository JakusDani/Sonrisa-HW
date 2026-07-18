using Api.Common.Data;
using Api.Common.Domain;
using Api.Features.Alerts.GetAlerts;
using Shouldly;
using Xunit;

namespace Api.Tests.Features.Alerts.GetAlerts;

public class GetAlertByIdHandlerTests
{
    [Fact]
    public void Handle_ReturnsNull_WhenAlertDoesNotExist()
    {
        var database = new FakeDatabase();

        var response = GetAlertByIdHandler.Handle(new GetAlertByIdQuery(Guid.NewGuid()), database);

        response.ShouldBeNull();
    }

    [Fact]
    public void Handle_ReturnsAlert_WhenItExists()
    {
        var database = new FakeDatabase();
        var alert = new Alert
        {
            Id = Guid.NewGuid(),
            Name = "Alert",
            EventType = EventType.NaturalDisaster,
            EnabledChannels = new[] { ChannelType.Slack },
            IsEnabled = true
        };
        database.AddAlert(alert);

        var response = GetAlertByIdHandler.Handle(new GetAlertByIdQuery(alert.Id), database);

        response.ShouldNotBeNull();
        response.Name.ShouldBe(alert.Name);
    }
}
