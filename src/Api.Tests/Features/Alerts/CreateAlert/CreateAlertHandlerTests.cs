using Api.Common.Data;
using Api.Common.Domain;
using Api.Features.Alerts.CreateAlert;
using FluentAssertions;
using Shouldly;
using Xunit;

namespace Api.Tests.Features.Alerts.CreateAlert;

public class CreateAlertHandlerTests
{
    [Fact]
    public void Handle_CreatesAlert_WhenCommandIsValid()
    {
        var database = new FakeDatabase();
        var command = new CreateAlertCommand("Alert", EventType.MarketMovement, new[] { ChannelType.Email });

        var response = CreateAlertHandler.Handle(command, database);

        response.Name.ShouldBe("Alert");
        response.IsEnabled.ShouldBeTrue();
        database.Alerts.Should().Contain(a => a.Id == response.Id);
    }

    [Fact]
    public void Handle_Throws_WhenNameIsMissing()
    {
        var database = new FakeDatabase();
        var command = new CreateAlertCommand(string.Empty, EventType.MarketMovement, new[] { ChannelType.Email });

        Should.Throw<ArgumentException>(() => CreateAlertHandler.Handle(command, database));
    }

    [Fact]
    public void Handle_Throws_WhenNoChannelsEnabled()
    {
        var database = new FakeDatabase();
        var command = new CreateAlertCommand("Alert", EventType.MarketMovement, Array.Empty<ChannelType>());

        Should.Throw<ArgumentException>(() => CreateAlertHandler.Handle(command, database));
    }
}
