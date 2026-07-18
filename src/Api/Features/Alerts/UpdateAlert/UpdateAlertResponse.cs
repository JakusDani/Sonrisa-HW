using Api.Common.Domain;

namespace Api.Features.Alerts.UpdateAlert;

public record UpdateAlertResponse(Guid Id, string Name, EventType EventType, IReadOnlyCollection<ChannelType> EnabledChannels, bool IsEnabled);
