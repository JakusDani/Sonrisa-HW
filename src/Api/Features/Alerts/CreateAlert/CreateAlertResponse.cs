using Api.Common.Domain;

namespace Api.Features.Alerts.CreateAlert;

public record CreateAlertResponse(Guid Id, string Name, EventType EventType, IReadOnlyCollection<ChannelType> EnabledChannels, bool IsEnabled);
