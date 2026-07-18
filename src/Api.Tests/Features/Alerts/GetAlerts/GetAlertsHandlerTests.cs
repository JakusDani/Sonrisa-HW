using Api.Common.Data;
using Api.Common.Domain;
using Api.Features.Alerts.GetAlerts;
using FluentAssertions;
using Shouldly;
using Xunit;

namespace Api.Tests.Features.Alerts.GetAlerts;

public class GetAlertsHandlerTests
{
    [Fact]
    public void Handle_ReturnsEmptyCollection_WhenNoAlertsExist()
    {
        var database = new FakeDatabase();

        var responses = GetAlertsHandler.Handle(new GetAlertsQuery(), database);

        responses.ShouldBeEmpty();
    }

    [Fact]
    public void Handle_ReturnsAllAlerts_WhenAlertsExist()
    {
        var database = new FakeDatabase();
        var alert = new Alert
        {
            Id = Guid.NewGuid(),
            Name = "Alert",
            EventType = EventType.BreakingNews,
            EnabledChannels = new[] { ChannelType.Email },
            IsEnabled = true
        };
        database.AddAlert(alert);

        var responses = GetAlertsHandler.Handle(new GetAlertsQuery(), database);

        responses.Should().ContainSingle(r => r.Id == alert.Id);
    }
}
