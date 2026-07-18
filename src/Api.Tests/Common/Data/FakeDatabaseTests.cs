using Api.Common.Data;
using Api.Common.Domain;
using FluentAssertions;
using Shouldly;
using Xunit;

namespace Api.Tests.Common.Data;

public class FakeDatabaseTests
{
    private static Alert _CreateAlert(string name = "Test Alert", EventType eventType = EventType.BreakingNews, bool isEnabled = true)
    {
        var alert = new Alert
        {
            Id = Guid.NewGuid(),
            Name = name,
            EventType = eventType,
            EnabledChannels = new[] { ChannelType.Email },
            IsEnabled = isEnabled
        };
        return alert;
    }

    [Fact]
    public void AddAlert_StoresAlert_RetrievableViaAlertsCollection()
    {
        var database = new FakeDatabase();
        var alert = _CreateAlert();

        database.AddAlert(alert);

        database.Alerts.Should().Contain(a => a.Id == alert.Id);
    }

    [Fact]
    public void GetAlert_ReturnsNull_WhenAlertDoesNotExist()
    {
        var database = new FakeDatabase();

        var result = database.GetAlert(Guid.NewGuid());

        result.ShouldBeNull();
    }

    [Fact]
    public void GetAlert_ReturnsAlert_WhenItExists()
    {
        var database = new FakeDatabase();
        var alert = _CreateAlert();
        database.AddAlert(alert);

        var result = database.GetAlert(alert.Id);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(alert.Name);
    }

    [Fact]
    public void RemoveAlert_ReturnsTrue_WhenAlertExisted()
    {
        var database = new FakeDatabase();
        var alert = _CreateAlert();
        database.AddAlert(alert);

        var removed = database.RemoveAlert(alert.Id);

        removed.ShouldBeTrue();
        database.Alerts.Should().NotContain(a => a.Id == alert.Id);
    }

    [Fact]
    public void RemoveAlert_ReturnsFalse_WhenAlertDidNotExist()
    {
        var database = new FakeDatabase();

        var removed = database.RemoveAlert(Guid.NewGuid());

        removed.ShouldBeFalse();
    }

    [Fact]
    public void UpdateAlert_AppliesUpdater_WhenAlertExists()
    {
        var database = new FakeDatabase();
        var alert = _CreateAlert(name: "Original");
        database.AddAlert(alert);

        var updated = database.UpdateAlert(alert.Id, existing => new Alert
        {
            Id = existing.Id,
            Name = "Updated",
            EventType = existing.EventType,
            EnabledChannels = existing.EnabledChannels,
            IsEnabled = existing.IsEnabled
        });

        updated.ShouldNotBeNull();
        updated.Name.ShouldBe("Updated");
    }

    [Fact]
    public void UpdateAlert_ReturnsNull_WhenAlertDoesNotExist()
    {
        var database = new FakeDatabase();

        var updated = database.UpdateAlert(Guid.NewGuid(), existing => existing);

        updated.ShouldBeNull();
    }

    [Fact]
    public void AddWorldEvent_StoresEvent_RetrievableViaWorldEventsCollection()
    {
        var database = new FakeDatabase();
        var worldEvent = new WorldEvent(Guid.NewGuid(), EventType.MarketMovement, "Message", DateTime.UtcNow);

        database.AddWorldEvent(worldEvent);

        database.WorldEvents.Should().Contain(e => e.Id == worldEvent.Id);
    }

    [Fact]
    public void AddNotificationLog_StoresLog_RetrievableViaNotificationLogsCollection()
    {
        var database = new FakeDatabase();
        var log = new NotificationLog(Guid.NewGuid(), Guid.NewGuid(), "Alert", ChannelType.Slack, "Message", NotificationStatus.Success, DateTime.UtcNow);

        database.AddNotificationLog(log);

        database.NotificationLogs.Should().Contain(l => l.Id == log.Id);
    }
}
