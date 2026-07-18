using Api.Common.Data;
using Api.Common.Domain;
using Api.Features.Alerts.DeleteAlert;
using FluentAssertions;
using Shouldly;
using Xunit;

namespace Api.Tests.Features.Alerts.DeleteAlert;

public class DeleteAlertHandlerTests
{
    [Fact]
    public void Handle_ReturnsDeletedTrue_WhenAlertExists()
    {
        var database = new FakeDatabase();
        var alert = new Alert
        {
            Id = Guid.NewGuid(),
            Name = "Alert",
            EventType = EventType.Custom,
            EnabledChannels = new[] { ChannelType.Email },
            IsEnabled = true
        };
        database.AddAlert(alert);

        var response = DeleteAlertHandler.Handle(new DeleteAlertCommand(alert.Id), database);

        response.Deleted.ShouldBeTrue();
        database.Alerts.Should().NotContain(a => a.Id == alert.Id);
    }

    [Fact]
    public void Handle_ReturnsDeletedFalse_WhenAlertDoesNotExist()
    {
        var database = new FakeDatabase();

        var response = DeleteAlertHandler.Handle(new DeleteAlertCommand(Guid.NewGuid()), database);

        response.Deleted.ShouldBeFalse();
    }
}
