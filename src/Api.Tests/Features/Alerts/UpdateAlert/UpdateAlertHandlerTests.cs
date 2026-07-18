using Api.Common.Data;
using Api.Common.Domain;
using Api.Features.Alerts.UpdateAlert;
using Shouldly;
using Xunit;

namespace Api.Tests.Features.Alerts.UpdateAlert;

public class UpdateAlertHandlerTests
{
    [Fact]
    public void Handle_UpdatesAlert_WhenCommandIsValidAndAlertExists()
    {
        var database = new FakeDatabase();
        var alert = new Alert
        {
            Id = Guid.NewGuid(),
            Name = "Original",
            EventType = EventType.BreakingNews,
            EnabledChannels = new[] { ChannelType.Email },
            IsEnabled = true
        };
        database.AddAlert(alert);
        var command = new UpdateAlertCommand(alert.Id, "Updated", EventType.MarketMovement, new[] { ChannelType.Slack }, false);

        var response = UpdateAlertHandler.Handle(command, database);

        response.ShouldNotBeNull();
        response.Name.ShouldBe("Updated");
        response.EventType.ShouldBe(EventType.MarketMovement);
        response.IsEnabled.ShouldBeFalse();
    }

    [Fact]
    public void Handle_ReturnsNull_WhenAlertDoesNotExist()
    {
        var database = new FakeDatabase();
        var command = new UpdateAlertCommand(Guid.NewGuid(), "Name", EventType.BreakingNews, new[] { ChannelType.Email }, true);

        var response = UpdateAlertHandler.Handle(command, database);

        response.ShouldBeNull();
    }

    [Fact]
    public void Handle_Throws_WhenNameIsMissing()
    {
        var database = new FakeDatabase();
        var alert = new Alert
        {
            Id = Guid.NewGuid(),
            Name = "Original",
            EventType = EventType.BreakingNews,
            EnabledChannels = new[] { ChannelType.Email },
            IsEnabled = true
        };
        database.AddAlert(alert);
        var command = new UpdateAlertCommand(alert.Id, string.Empty, EventType.BreakingNews, new[] { ChannelType.Email }, true);

        Should.Throw<ArgumentException>(() => UpdateAlertHandler.Handle(command, database));
    }

    [Fact]
    public void Handle_Throws_WhenNoChannelsEnabled()
    {
        var database = new FakeDatabase();
        var alert = new Alert
        {
            Id = Guid.NewGuid(),
            Name = "Original",
            EventType = EventType.BreakingNews,
            EnabledChannels = new[] { ChannelType.Email },
            IsEnabled = true
        };
        database.AddAlert(alert);
        var command = new UpdateAlertCommand(alert.Id, "Name", EventType.BreakingNews, Array.Empty<ChannelType>(), true);

        Should.Throw<ArgumentException>(() => UpdateAlertHandler.Handle(command, database));
    }
}
