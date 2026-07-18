using Api.Common.Domain;

namespace Api.Features.Alerts.GetAlerts;

public record AlertResponse(Guid Id, string Name, EventType EventType, IReadOnlyCollection<ChannelType> EnabledChannels, bool IsEnabled);
