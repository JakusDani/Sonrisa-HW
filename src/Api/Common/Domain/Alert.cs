namespace Api.Common.Domain;

public class Alert
{
    public Guid Id { get; init; }
    public required string Name { get; set; }
    public EventType EventType { get; set; }
    public required IReadOnlyCollection<ChannelType> EnabledChannels { get; set; }
    public bool IsEnabled { get; set; }
}
