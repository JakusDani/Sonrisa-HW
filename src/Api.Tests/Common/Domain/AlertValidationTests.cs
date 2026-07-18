using Api.Common.Domain;
using Shouldly;
using Xunit;

namespace Api.Tests.Common.Domain;

public class AlertValidationTests
{
    [Fact]
    public void IsValid_ReturnsTrue_WhenNameAndChannelsProvided()
    {
        var valid = AlertValidation.IsValid("Alert Name", new[] { ChannelType.Email });

        valid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void IsValid_ReturnsFalse_WhenNameIsMissing(string? name)
    {
        var valid = AlertValidation.IsValid(name!, new[] { ChannelType.Email });

        valid.ShouldBeFalse();
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenNoChannelsEnabled()
    {
        var valid = AlertValidation.IsValid("Alert Name", Array.Empty<ChannelType>());

        valid.ShouldBeFalse();
    }
}
