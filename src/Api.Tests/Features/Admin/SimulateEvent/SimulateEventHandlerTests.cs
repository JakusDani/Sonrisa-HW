using Api.Common.Data;
using Api.Common.Domain;
using Api.Features.Admin.SimulateEvent;
using Api.Tests.TestDoubles;
using Shouldly;
using Xunit;

namespace Api.Tests.Features.Admin.SimulateEvent;

public class SimulateEventHandlerTests
{
    private static Alert _CreateAlert(EventType eventType, bool isEnabled, params ChannelType[] channels)
    {
        var alert = new Alert
        {
            Id = Guid.NewGuid(),
            Name = "Alert",
            EventType = eventType,
            EnabledChannels = channels,
            IsEnabled = isEnabled
        };
        return alert;
    }

    [Fact]
    public async Task Handle_StoresWorldEvent_AndReturnsZeroCounts_WhenNoAlertsMatch()
    {
        var database = new FakeDatabase();
        var queue = new RecordingNotificationQueue();
        var command = new SimulateEventCommand(EventType.BreakingNews, "Breaking!");

        var response = await SimulateEventHandler.Handle(command, database, queue);

        response.MatchedAlertsCount.ShouldBe(0);
        response.NotificationsQueuedCount.ShouldBe(0);
        database.WorldEvents.Count.ShouldBe(1);
    }

    [Fact]
    public async Task Handle_MatchesEnabledAlertsWithSameEventType_AndQueuesOnePerChannel()
    {
        var database = new FakeDatabase();
        var queue = new RecordingNotificationQueue();
        database.AddAlert(_CreateAlert(EventType.MarketMovement, true, ChannelType.Email, ChannelType.Slack));
        var command = new SimulateEventCommand(EventType.MarketMovement, "Market moved");

        var response = await SimulateEventHandler.Handle(command, database, queue);

        response.MatchedAlertsCount.ShouldBe(1);
        response.NotificationsQueuedCount.ShouldBe(2);
        queue.PublishedMessages.Count.ShouldBe(2);
    }

    [Fact]
    public async Task Handle_ExcludesDisabledAlerts_FromMatching()
    {
        var database = new FakeDatabase();
        var queue = new RecordingNotificationQueue();
        database.AddAlert(_CreateAlert(EventType.NaturalDisaster, false, ChannelType.Email));
        var command = new SimulateEventCommand(EventType.NaturalDisaster, "Earthquake");

        var response = await SimulateEventHandler.Handle(command, database, queue);

        response.MatchedAlertsCount.ShouldBe(0);
        response.NotificationsQueuedCount.ShouldBe(0);
    }

    [Fact]
    public async Task Handle_ExcludesAlerts_WithDifferentEventType()
    {
        var database = new FakeDatabase();
        var queue = new RecordingNotificationQueue();
        database.AddAlert(_CreateAlert(EventType.Custom, true, ChannelType.Email));
        var command = new SimulateEventCommand(EventType.BreakingNews, "Breaking!");

        var response = await SimulateEventHandler.Handle(command, database, queue);

        response.MatchedAlertsCount.ShouldBe(0);
        response.NotificationsQueuedCount.ShouldBe(0);
    }
}
